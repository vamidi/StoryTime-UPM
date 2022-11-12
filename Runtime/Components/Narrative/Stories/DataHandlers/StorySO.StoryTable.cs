
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using StoryTime.FirebaseService.Database;
using StoryTime.FirebaseService.Database.ResourceManagement;
using StoryTime.Utils.Attributes;
using StoryTime.VisualScripting.Data;
using StoryTime.VisualScripting.Data.ScriptableObjects;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Networking;

namespace StoryTime.Components.ScriptableObjects
{
	using FirebaseService.Database.Binary;

	// ReSharper disable once InconsistentNaming
	public partial class StorySO
	{
		[SerializeField, Tooltip("Override where we should get the dialogue options data from.")]
		protected bool overrideDialogueOptionsTable;

#if UNITY_EDITOR
		[SerializeField, ConditionalField("overrideDialogueOptionsTable"), Tooltip("Table collection we are going to use for the sentence")]
		protected StringTableCollection dialogueOptionsCollection;
#endif

		[SerializeField, Tooltip("Override where we should get the character data from.")]
		protected bool overrideCharacterTable;

#if UNITY_EDITOR
		[SerializeField, ConditionalField("overrideTable"), Tooltip("Table collection we are going to use")]
		protected StringTableCollection characterCollection;
#endif

		// Node editor stuff
		[SerializeField] internal StartNode rootNode;
		[SerializeField] internal List<Node> nodes = new ();
		[SerializeField] internal List<ExposedProperty> exposedProperties = new ();

		public Node CreateNode(Type type)
		{
			Node node = CreateInstance(type) as Node;
			node.name = type.Name;
#if UNITY_EDITOR
			node.guid = GUID.Generate().ToString();
#endif
			Undo.RecordObject(this, "Dialogue Graphview (Create Node)");
			nodes.Add(node);

#if UNITY_EDITOR
			AssetDatabase.AddObjectToAsset(node, this);
			Undo.RegisterCreatedObjectUndo(node, "Dialogue Graphview (Create Node)");
			AssetDatabase.SaveAssets();
#endif
			return node;
		}

		public void DeleteNode(Node node)
		{
			Undo.RecordObject(this, "Dialogue Graphview (Create Node)");
			nodes.Remove(node);
#if UNITY_EDITOR
			// AssetDatabase.RemoveObjectFromAsset(node);
			Undo.DestroyObjectImmediate(node);
			AssetDatabase.SaveAssets();
#endif
		}

		public void AddChild(Node parent, Node child)
		{
			if (parent is StartNode startNode)
			{
				Undo.RecordObject(startNode, "Dialogue Graphview (Add Child)");
				startNode.Child = child;
				EditorUtility.SetDirty(startNode);
			}

			if (parent is DialogueNode dialogueNode && !dialogueNode.Children.Contains(child))
			{
				Undo.RecordObject(dialogueNode, "Dialogue Graphview (Add Child)");
				dialogueNode.Children.Add(child);
				EditorUtility.SetDirty(dialogueNode);
			}
		}

		public void RemoveChild(Node parent, Node child)
		{
			if (parent is StartNode startNode)
			{
				Undo.RecordObject(startNode, "Dialogue Graphview (Remove Child)");
				startNode.Child = null;
				EditorUtility.SetDirty(startNode);
			}

			if (parent is DialogueNode dialogueNode)
			{
				Undo.RecordObject(dialogueNode, "Dialogue Graphview (Remove Child)");
				dialogueNode.Children.Remove(child);
				EditorUtility.SetDirty(dialogueNode);
			}
		}

		public List<Node> GetChildren(Node parent)
		{
			List<Node> children = new();
			if (parent is StartNode startNode && startNode.Child)
			{
				children.Add(startNode.Child);
			}

			if (parent is DialogueNode dialogueNode)
			{
				children = dialogueNode.Children;
			}

			return children;
		}

		public void Parse(FirebaseStorageService.StoryFileUpload storyFileUpload)
		{
			// dialogueLines.Clear();
			// Only get the first dialogue.
			rootNode.DialogueLine =
				DialogueLine.DialogueTable.ConvertRow(TableDatabase.Get.GetRow("dialogues", childId),
#if UNITY_EDITOR
					overrideTable
						? collection
						: LocalizationEditorSettings.GetStringTableCollection("Dialogues"),
					overrideCharacterTable
						? characterCollection
						: LocalizationEditorSettings.GetStringTableCollection("Character Names")
#endif
				);

			UnityWebRequest wr = UnityWebRequest.Get(storyFileUpload.url);

			wr.timeout = 60;
			// wr.SetRequestHeader("User-Agent", "X-Unity3D-Agent");
			// wr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
			wr.SendWebRequest().completed += (_) => ParseNodeData(wr);
		}

		/// <summary>
		///
		/// </summary>
		private void ParseNodeData(UnityWebRequest request)
		{
			if (request.result == UnityWebRequest.Result.ConnectionError || request.responseCode == 401 ||
			    request.responseCode == 500)
			{
#if UNITY_EDITOR
				Debug.LogWarningFormat("Error: {0}", request.error);
#else
				throw new ArgumentException("Error: ", wr.error);
#endif
			}

			// Handle result
			string str = request.downloadHandler.text;

			if(str == String.Empty)
			{
#if UNITY_EDITOR
				Debug.LogWarning("Node data not found!");
#else
				throw new ArgumentException("Node data not found!");
#endif
			}

			JObject jObject = JObject.Parse(str);
			if (jObject["nodes"] == null)
				return;

			JObject nodes = jObject["nodes"].ToObject<JObject>();

			if (nodes == null)
				return;

			// Retrieve the first node. because that is the start node.
			// if not debug show error.
			var nodeToken = nodes.First.Value<JProperty>();
			var node = new JObject();

			while (nodeToken != null)
			{
				node = nodeToken.Value.ToObject<JObject>();

				if (node["name"].ToObject<string>().ToLower() == "start")
					nodeToken = null; // we found the start node.
				else
				{
					node.RemoveAll();
					nodeToken = nodeToken.Next?.Value<JProperty>();
				}
			}

			if (!node.HasValues)
			{
				Debug.LogWarning("Start Node could not be found!");
				return;
			}

			// start with the start dialogue
			rootNode = CreateNode(typeof(StartNode)) as StartNode;

			if (rootNode)
			{
				// Instantiate a new dialogue line
				rootNode.DialogueLine = new ();
				// Debug.Log("Current" + currentDialogue);
				ParseNextNodeData(rootNode, node, nodes);
			}

		}

		// Override this function to make your custom converter
		private DialogueEventSO ConvertEvent(TableRow row, KeyValuePair<string, JToken> @event)
		{
			var eventName = "";
			// validate the data
			if (row.Fields.Count > 0)
			{
				var field = row.Find("name");
				if (field != null)
				{
					eventName = (string) field.Data;
				}
			}

			var valueToken = @event.Value["value"];

			if (valueToken == null)
			{
				Debug.Log("Value is null. Creating empty event");
				return new DialogueEventSO(eventName);
			}

			JToken token = valueToken["value"];
			if (token == null)
			{
				Debug.Log("Token is null. Creating empty event");
				return new DialogueEventSO(eventName);
			}

			dynamic value = null;
			switch (token.Type)
			{
				case JTokenType.Boolean:
					value = token.ToObject<bool>();
					Debug.Log("Creating bool value");
					break;
				case JTokenType.Integer:
					value = token.ToObject<double>();
					Debug.Log("Creating numeric value");
					break;
				case JTokenType.String:
					value = token.ToObject<string>();
					Debug.Log("Creating string value");
					break;
				case JTokenType.Object:
					Debug.LogError("You are trying to convert an object to a dynamic value");
					break;
			}

			return new DialogueEventSO(eventName, value);
		}

		/// <summary>
		/// Grab the next dialogue
		/// nextDialogue = ConvertRow(TableDatabase.Get.GetRow("dialogues", nextDialogueID));
		/// Set the dialogue options associated to the dialogue.
		/// CheckDialogueOptions(nextDialogue);
		/// so input has a connection member that consists of three values
		/// node -> reference to the other node.
		/// output -> reference to the key that consist of the value
		/// so in order to grab the data go to the node
		/// fetch the data
		/// if it is an option take options[$`optionOut-key`] and then the value
		/// if it is a dialogue take data["dialogueId"] -> can be Number.MAX_SAFE_INTEGER
		/// so output has a connection member that consists of three values
		/// node -> reference to the other node.
		/// input -> reference to the key that consist of the value
		/// so in order to grab the data go to the node
		/// fetch the data
		/// if it is an option take options[$`optionOut-key`] and then the value
		/// if it is a dialogue take data["dialogueId"] -> can be Number.MAX_SAFE_INTEGER
		/// </summary>
		/// <param name="currentNode"></param>
		/// <param name="node"></param>
		/// <param name="nodes"></param>
		private void ParseNextNodeData(IDialogueNode currentNode, JObject node, JObject nodes)
		{
			if (node["data"] == null)
				return;

			var data = node["data"].ToObject<JObject>();

			// check what is inside the node
			if (data != null)
			{
				// get the outputs
				var outputs = node["outputs"].ToObject<JObject>();

				// {"id":"story@0.2.0","nodes":{"1":{"id":1,"data":{"dialogueId":0},"inputs":{},"outputs":{"dialogueOut":{"connections":[]}},"position":[200,200],"name":"Start"}},"comments":[],"characters":[3]}

				// loop through the outputs
				// Outputs can be
				// Dialogue to dialogue
				// option to dialogue
				foreach (var outputToken in outputs)
				{
					var output = outputToken.Value.ToObject<JObject>();
					var connections = output["connections"].ToArray();
					var emptyObj = new JObject();

					string nodeId;
					JObject otherNode;
					JObject otherData;

					// Fetch event data
					if (outputToken.Key.Contains("Exec") && connections.Length > 0)
					{
						foreach (var con in connections)
						{
							// grab the other node id.
							nodeId = con["node"]?.ToObject<int>().ToString() ?? String.Empty;
							// grab the other node object.
							otherNode = nodeId != String.Empty ? nodes[nodeId].Value<JObject>() : emptyObj;
							// grab the data from the other node.
							otherData = otherNode["data"]?.ToObject<JObject>() ?? emptyObj;

							if (currentNode.DialogueLine != null)
							{
								// fetch the event name
								var eventId = otherData["eventId"]?.ToObject<uint>() ?? UInt32.MaxValue;
								if (eventId != UInt32.MaxValue)
								{
									var row = TableDatabase.Get.GetRow("events", eventId);

									// fetch the parameters
									//  TODO mark event value as dynamic to support multiple parameters
									JObject events = otherData["events"]?.ToObject<JObject>() ?? emptyObj;
									foreach (var @event in events)
									{
										currentNode.DialogueLine.DialogueEvent = ConvertEvent(row, @event);
									}
								}
							}
						}
					}

					// see if we have a connection
					// if (connections.Length == 0)
					// continue;

					// See if we are dealing with an option
					bool containsOption = outputToken.Key.Contains("option");

					var connection = connections.Length > 0 ? connections[0] : emptyObj;
					// grab the other node id.
					nodeId = connection["node"]?.ToObject<int>().ToString() ?? String.Empty;
					// grab the other node object.
					otherNode = nodeId != String.Empty ? nodes[nodeId].Value<JObject>() : emptyObj;
					// grab the data from the other node.
					otherData = otherNode["data"]?.ToObject<JObject>() ?? emptyObj;

					IDialogueNode nextDialogue = null;
					if (currentNode.DialogueLine != null)
					{
						// Fetch the other dialogueId
						var nextId = otherData["dialogueId"]?.ToObject<uint>() ?? UInt32.MaxValue;

						// if this node does not consist of any choices
						// go this way
						if (!containsOption)
						{
							// validate the data
							if (nextId != UInt32.MaxValue)
							{
								nextDialogue = CreateNode(typeof(DialogueNode)) as DialogueNode;
								nextDialogue.DialogueLine = DialogueLine.DialogueTable.ConvertRow(
									TableDatabase.Get.GetRow("dialogues", nextId),
#if UNITY_EDITOR
									overrideTable
										? collection
										: LocalizationEditorSettings.GetStringTableCollection("Dialogues"),
									overrideCharacterTable
										? characterCollection
										: LocalizationEditorSettings.GetStringTableCollection("Character Names")
#endif
								);
							}

							// Debug.Log(" Next: " + currentDialogue.NextDialogue);

							// now we have the next id check if we have a node that comes after.
							ParseNextNodeData(nextDialogue, otherNode, nodes);
						}
						else
						{
							// grab the choice id from the current node.
							var optionId = data["options"][outputToken.Key]["value"].ToObject<uint>();

							// Grab the choice
							DialogueChoice choice = DialogueChoice.ChoiceTable.ConvertRow(
								TableDatabase.Get.GetRow("dialogueOptions", optionId),
								overrideDialogueOptionsTable
									? dialogueOptionsCollection
									: LocalizationEditorSettings.GetStringTableCollection("DialogueOptions")
							);

							// find the next dialogue of this choice.

							if (nextId != UInt32.MaxValue)
							{
								currentNode.DialogueLine =
									DialogueLine.DialogueTable.ConvertRow(TableDatabase.Get.GetRow("dialogues", nextId),
#if UNITY_EDITOR
										overrideTable
											? collection
											: LocalizationEditorSettings.GetStringTableCollection("Dialogues"),
										overrideCharacterTable
											? characterCollection
											: LocalizationEditorSettings.GetStringTableCollection("Character Names")
#endif
									);

								// create a new line for the choice selected.
								var dialogueNode = CreateNode(typeof(DialogueNode)) as DialogueNode;

								dialogueNode.DialogueLine = currentNode.DialogueLine;
							}

							// Debug.Log(" Choice: " + choice);

							// add the choices to the currentDialogue
							if (nextDialogue is DialogueNode dn)
							{
								dn.Choices.Add(choice);
							}

							// Set the nextDialogue to null because we are dealing with a choice
							nextDialogue = null;

							// Find the next dialogue for the choice
							ParseNextNodeData(nextDialogue, otherNode, nodes);
						}
					}
				}
			}
		}

		public class StoryTable : BaseTable<StorySO>
		{
			public new static StorySO ConvertRow(TableRow row, StorySO scriptableObject = null)
			{
				StorySO story = scriptableObject ? scriptableObject : CreateInstance<StorySO>();

				if (row.Fields.Count == 0)
				{
					return story;
				}

				story.ID = row.RowId;

				foreach (var field in row.Fields)
				{
					if (field.Key.Equals("id"))
					{
						uint data = (uint) field.Value.Data;
						story.ID = data == uint.MaxValue - 1 ? uint.MaxValue : data;
					}

					// Fetch the first dialogue we should start
					if (field.Key.Equals("childId"))
					{
						// retrieve the necessary items
						uint data = (uint) field.Value.Data;
						story.childId = data == uint.MaxValue - 1 ? uint.MaxValue : data;
					}

					if (field.Key.Equals("parentId"))
					{
						// retrieve the necessary items
						uint data = (uint) field.Value.Data;
						story.parentId = data == uint.MaxValue - 1 ? uint.MaxValue : data;
					}

					if (field.Key.Equals("typeId"))
					{
						story.typeId = (StoryType) field.Value.Data;
					}
				}

				return story;
			}
		}
	}
}
