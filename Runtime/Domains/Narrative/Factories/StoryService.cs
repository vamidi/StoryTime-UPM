using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Zenject;
using Newtonsoft.Json.Linq;


namespace StoryTime.Domains.Narrative.Factories
{
	using VisualScripting;
	using StoryTime.Domains.Database;
	using StoryTime.Domains.Database.Binary;
	using StoryTime.FirebaseService.Database.ResourceManagement;
	using StoryTime.Domains.VisualScripting.Data.ScriptableObjects;
	using StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Events;
	using StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues;
	
    public class StoryFactory
    {
	    private readonly NodeEditorService _service;
        
	    // Node editor stuff

	    [Inject]
	    public StoryFactory(NodeEditorService service)
	    {
		    this._service = service;
	    }

		public void Parse(FirebaseStorageService.StoryFileUpload storyFileUpload, Node rootNode)
		{
			// dialogueLines.Clear();

			UnityWebRequest wr = UnityWebRequest.Get(storyFileUpload.url);

			wr.timeout = 60;
			// wr.SetRequestHeader("User-Agent", "X-Unity3D-Agent");
			// wr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
			wr.SendWebRequest().completed += (_) => ParseNodeData(wr, new (), rootNode);
		}

		/// <summary>
		///
		/// </summary>
		private void ParseNodeData(UnityWebRequest request, NodeCollection nodeCollection, Node rootNode)
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
			rootNode = _service.CreateNode(typeof(StartNode), ref nodeCollection) as StartNode;

			if (rootNode)
			{
				// Instantiate a new dialogue line
				// Only get the first dialogue.
				/* TODO make this work
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
				*/
				// Debug.Log("Current" + currentDialogue);
				ParseNextNodeData(rootNode, node, nodes, ref nodeCollection);
			}

		}

		// Override this function to make your custom converter
		private EventNode ConvertEvent(TableRow row, KeyValuePair<string, JToken> @event, ref NodeCollection nodeCollection)
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

			EventNode node = null;
			if (valueToken == null)
			{
				Debug.Log("Value is null. Creating empty event");
				node = _service.CreateNode(typeof(EventNode<>), ref nodeCollection) as EventNode;
				// node.EventName = eventName;
				return node;
			}

			JToken token = valueToken["value"];
			if (token == null)
			{
				Debug.Log("Token is null. Creating empty event");
				node = _service.CreateNode(typeof(EventNode<>), ref nodeCollection) as EventNode;
				// node.EventName = eventName;
				return node;
			}

			switch (token.Type)
			{
				case JTokenType.Boolean:
					var boolEventNode = _service.CreateNode(typeof(BoolEventNode), ref nodeCollection) as BoolEventNode;
					boolEventNode.EventName = eventName;
					boolEventNode.Value = token.ToObject<bool>();
					node = boolEventNode;
					Debug.Log("Creating bool value");
					break;
				case JTokenType.Integer:
					var intEventNode = _service.CreateNode(typeof(IntEventNode), ref nodeCollection) as EventNode<IComparable>;
					intEventNode.EventName = eventName;
					intEventNode.Value = token.ToObject<double>();
					node = intEventNode;
					Debug.Log("Creating numeric value");
					break;
				case JTokenType.String:
					var stringEventNode = _service.CreateNode(typeof(StringEventNode), ref nodeCollection) as EventNode<IComparable>;
					stringEventNode.EventName = eventName;
					stringEventNode.Value = token.ToObject<string>();
					node = stringEventNode;
					Debug.Log("Creating string value");
					break;
				case JTokenType.Object:
					Debug.LogError("You are trying to convert an object to a dynamic value");
					break;
			}

			return node;
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
		private void ParseNextNodeData(Node currentNode, JObject node, JObject nodes, ref NodeCollection nodeCollection)
		{
			if (node["data"] == null)
				return;

			var data = node["data"].ToObject<JObject>();

			// check what is inside the node
			if (data != null)
			{
				var currentDialogueNode = currentNode as DialogueNode;

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

					Node nextNode = null;
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

							if (currentDialogueNode && currentDialogueNode.DialogueLine != null)
							{
								// fetch the event name
								var eventId = otherData["eventId"]?.ToObject<String>() ?? String.Empty;
								if (eventId != String.Empty)
								{
									var row = TableDatabase.Get.GetRow("events", eventId);

									// fetch the parameters
									//  TODO mark event value as dynamic to support multiple parameters
									JObject events = otherData["events"]?.ToObject<JObject>() ?? emptyObj;
									foreach (var @event in events)
									{
										nextNode = ConvertEvent(row, @event, ref nodeCollection);
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

					if ( currentDialogueNode && currentDialogueNode.DialogueLine != null)
					{
						// Fetch the other dialogueId
						var nextId = otherData["dialogueId"]?.ToObject<string>() ?? String.Empty;

						// if this node does not consist of any choices
						// go this way
						if (!containsOption)
						{
							// validate the data
							if (nextId != String.Empty)
							{
								var nextDialogue = _service.CreateNode(typeof(DialogueNode), ref nodeCollection) as DialogueNode;
								/* TODO make this work
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
								*/
								nextNode = nextDialogue;
							}

							// Debug.Log(" Next: " + currentDialogue.NextDialogue);

							// now we have the next id check if we have a node that comes after.
							ParseNextNodeData(nextNode, otherNode, nodes, ref nodeCollection);
						}
						else
						{
							// grab the choice id from the current node.
							var optionId = data["options"][outputToken.Key]["value"].ToObject<String>();

							// Grab the choice
							/* TODO make this work 
							DialogueChoice choice = DialogueChoice.ChoiceTable.ConvertRow(
								TableDatabase.Get.GetRow("dialogueOptions", optionId),
								overrideDialogueOptionsTable
									? dialogueOptionsCollection
									: LocalizationEditorSettings.GetStringTableCollection("DialogueOptions")
							);
							*/

							// find the next dialogue of this choice.

							if (nextId != String.Empty)
							{
								/* TODO make this work
								currentDialogueNode.DialogueLine =
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
								*/

								// create a new line for the choice selected.
								var dialogueNode = _service.CreateNode(typeof(DialogueNode), ref nodeCollection) as DialogueNode;

								// dialogueNode.DialogueLine = currentDialogueNode.DialogueLine;
							}

							// Debug.Log(" Choice: " + choice);

							// add the choices to the currentDialogue
							if (nextNode is DialogueNode dn)
							{
								// TODO make this work
								// dn.Choices.Add(choice);
							}

							// Set the nextDialogue to null because we are dealing with a choice
							nextNode = null;

							// Find the next dialogue for the choice
							ParseNextNodeData(nextNode, otherNode, nodes, ref nodeCollection);
						}
					}
				}
			}
		}
    }
}