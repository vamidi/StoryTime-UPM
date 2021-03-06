using System;
using System.Linq;

using Newtonsoft.Json.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace DatabaseSync.Components
{
	using Database;
	using Binary;

	// ReSharper disable once InconsistentNaming
	public partial class StorySO
	{
		public class StoryTable : BaseTable<StorySO>
		{
			public new static StorySO ConvertRow(TableRow row, StorySO scriptableObject = null)
			{
				StorySO story = scriptableObject ? scriptableObject : CreateInstance<StorySO>();

				if (row.Fields.Count == 0)
				{
					return story;
				}

				DatabaseConfig config = TableBinary.Fetch();
				if (config != null)
				{
					story.ID = row.RowId;
					var entryId = (story.ID + 1).ToString();
					if (!story.title.IsEmpty)
						story.title.TableEntryReference = entryId;

					if(!story.Description.IsEmpty)
						story.Description.TableEntryReference = entryId;
				}

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

			/// <summary>
			///
			/// </summary>
			public static void ParseNodeData(StorySO story, JObject jObject)
			{
				if (jObject["nodes"] == null)
					return;

				JObject nodes = jObject["nodes"].ToObject<JObject>();

				// Retrieve the first node. because that is the start node.
				// if not debug show error.
				var nodeToken = nodes.First.Value<JProperty>();

				var node = nodeToken.Value.ToObject<JObject>();
				if (node["name"].ToObject<string>().ToLower() != "start")
				{
					Debug.LogWarning("First Node is not the start node");
					return;
				}

				// start with the start dialogue
				DialogueLine currentDialogue = story.startDialogue;

				// Debug.Log("Current" + currentDialogue);

				ParseNextNodeData(story, currentDialogue, node, nodes);
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
			/// <param name="currentDialogue"></param>
			/// <param name="story"></param>
			/// <param name="node"></param>
			/// <param name="nodes"></param>
			private static void ParseNextNodeData(StorySO story, IDialogueLine currentDialogue, JObject node, JObject nodes)
			{
				if (node["data"] == null)
					return;

				var data = node["data"].ToObject<JObject>();

				// check what is inside the node
				if (data != null)
				{
					// get the outputs
					var outputs = node["outputs"].ToObject<JObject>();

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
						if (outputToken.Key.Contains("Exec") && connections.Length > 0)
						{
							foreach (var con in connections)
							{
								// grab the other node id.
								nodeId =  con["node"]?.ToObject<int>().ToString() ?? String.Empty;
								// grab the other node object.
								otherNode = nodeId != String.Empty ? nodes[nodeId].Value<JObject>() : emptyObj;
								// grab the data from the other node.
								otherData = otherNode["data"]?.ToObject<JObject>() ?? emptyObj;

								if (currentDialogue != null)
								{
									// fetch the event name
									var eventId = otherData["eventId"]?.ToObject<uint>() ?? UInt32.MaxValue;
									var eventName = "";
									if (eventId != UInt32.MaxValue)
									{
										var row = TableDatabase.Get.GetRow("events", eventId);

										// validate the data
										if (row.Fields.Count > 0)
										{
											var field = row.Find("name");
											if (field != null)
											{
												eventName = (string) field.Data;
											}
										}
										// fetch the parameters
										//  TODO mark event value as dynamic to support multiple parameters
										JObject events = otherData["events"]?.ToObject<JObject>() ?? emptyObj;
										foreach (var @event in events)
										{
											// int value = @event.Value["value"]["value"].ToObject<int>();
											currentDialogue.DialogueEvent = new DialogueEventSO(eventName, story);
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
						nodeId =  connection["node"]?.ToObject<int>().ToString() ?? String.Empty;
						// grab the other node object.
						otherNode = nodeId != String.Empty ? nodes[nodeId].Value<JObject>() : emptyObj;
						// grab the data from the other node.
						otherData = otherNode["data"]?.ToObject<JObject>() ?? emptyObj;

						if (currentDialogue != null)
						{
							// Fetch the other dialogueId
							var nextId = otherData["dialogueId"]?.ToObject<uint>() ?? UInt32.MaxValue;

							// if this node does not consist of any choices
							// go this way
							if (!containsOption)
							{
								// validate the data
								currentDialogue.NextDialogue = nextId != UInt32.MaxValue ?
									DialogueLine.ConvertRow(TableDatabase.Get.GetRow("dialogues", nextId),
										story.overrideTable ? story.collection : LocalizationEditorSettings.GetStringTableCollection("Dialogues"))
									: null;

								// Debug.Log(" Next: " + currentDialogue.NextDialogue);

								// now we have the next id check if we have a node that comes after.
								ParseNextNodeData(story, currentDialogue.NextDialogue, otherNode, nodes);
							}
							else
							{
								// grab the choice id from the current node.
								var optionId = data["options"][outputToken.Key]["value"].ToObject<uint>();

								// Grab the choice
								DialogueChoiceSO choice = DialogueChoiceSO.ConvertRow(TableDatabase.Get.GetRow("dialogueOptions", optionId),
									story.overrideDialogueOptionsTable ? story.dialogueOptionsCollection : LocalizationEditorSettings.GetStringTableCollection("DialogueOptions")
								);

								// find the next dialogue of this choice.
								choice.NextDialogue = nextId != UInt32.MaxValue ?
									DialogueLine.ConvertRow(TableDatabase.Get.GetRow("dialogues", nextId),
										story.overrideTable ? story.collection : LocalizationEditorSettings.GetStringTableCollection("Dialogues"))
									: null;

								// Debug.Log(" Choice: " + choice);

								// add the choices to the currentDialogue
								currentDialogue.Choices.Add(choice);

								// Set the nextDialogue to null because we are dealing with a choice
								currentDialogue.NextDialogue = null;

								// Find the next dialogue for the choice
								ParseNextNodeData(story, choice.NextDialogue, otherNode, nodes);
							}
						}
					}
				}
			}
		}
	}
}
