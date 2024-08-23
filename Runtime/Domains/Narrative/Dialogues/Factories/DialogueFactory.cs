using System;

using UnityEditor.Localization;

using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Narrative.Dialogues.Factories
{
	using StoryTime.Domains.Database.Binary;
	
	public class DialogueFactory
	{
		public static DialogueLine ConvertRow(TableRow row,
#if UNITY_EDITOR
			StringTableCollection dialogueCollection,
			StringTableCollection characterCollection,
#endif
			DialogueLine dialogueLine = null)
		{
			DialogueLine dialogue = dialogueLine ?? new DialogueLine();

			if (row.Fields.Count == 0)
			{
				return dialogue;
			}

			dialogue.ID = row.RowId;
			var entryId = (dialogue.ID + 1).ToString();
			// TODO fixme
			// if (dialogueCollection)
				// dialogue.sentence = new LocalizedString
					// { TableReference = dialogueCollection.TableCollectionNameReference, TableEntryReference = entryId };
			// else
				// Debug.LogWarning("Collection not found. Did you create any localization tables");

			foreach (var field in row.Fields)
			{
				/*
				if (field.Key.Equals("nextId"))
				{
					uint data = (uint)field.Value.Data;
					dialogue.nextDialogueID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}
				*/

				if (field.Key.Equals("characterId"))
				{
					uint data = (uint)field.Value.Data;
					uint characterID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;

					if (characterID != UInt32.MaxValue)
					{
						characterID++;
						if (characterCollection)
						{
							// TODO fixme
							var characterName
								/* dialogue.speaker.CharacterName */ = new LocalizedString
								{
									TableReference = characterCollection.TableCollectionNameReference,
									TableEntryReference = characterID
								};
						}
						else
							Debug.LogWarning("Collection not found. Did you create any localization tables");
					}
				}

				/*
				if (field.Key.Equals("parentId"))
				{
					uint data = (uint) field.Value.Data;
					dialogue.parentId = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}
				*/
			}

			return dialogue;
		}

		/// <summary>
		/// Converts a row from the json file to a dialogue option.
		/// This can then be immediately use in the game.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="collection"></param>
		/// <param name="option"></param>
		/// <returns>DialogueOption</returns>
		public static DialogueChoice ConvertRow(TableRow row, StringTableCollection collection,
			DialogueChoice option = null)
		{
			// Make an empty object.
			DialogueChoice dialogueOption = option ?? new DialogueChoice();

			// If we have no fields return the base object.
			if (row.Fields.Count == 0)
			{
				return dialogueOption;
			}

			if (collection != null)
			{
				dialogueOption.ID = row.RowId;
				var entryId = (dialogueOption.ID + 1).ToString();
				
				// TODO fixme
				// if (collection)
					// dialogueOption.text = new LocalizedString
						// { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				// else
					// Debug.LogWarning("Collection not found. Did you create any localization tables");
			}

			// Loop through all the fields and acquire the right data
			// TODO make an interop to let users make their own functions
			foreach (var field in row.Fields)
			{
				/*
				if (field.Key.Equals("id"))
				{
					dialogueOption.ID = (uint) field.Value.Data;
				}
				*/

				// TODO comes from the node editor
				// if (field.Key.Equals("childId"))
				// {
				// uint data = (uint) field.Value.Data;
				// dialogueOption.ChildId = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				// }

				// if (field.Key.Equals("text"))
				// {
				// 	Debug.Log(field.Value.Data);
				// }
/*
				// TODO make regular expression work in dialogue options
				if (field.Key.Equals("text"))
				{
					var data = (string) field.Value.Data;
					Match regexMatch = HelperClass.GetActionRegex(@"<action=(.*?)>", data);
					// the indices needs to match either
					if (regexMatch.Success)
					{
						Match match = HelperClass.GetActionRegex(@"(?<=<action=)(.*?)(?=>)", data);
						dialogueOption.eventName = match.Value;

						// remove all the action data
						data = Regex.Replace(data, "<action=(.*?)>", "", RegexOptions.Singleline);
					}

					// TODO get the entry of the string table
					// dialogueOption.text = data;
				}
*/
			}

			return dialogueOption;
		}
	}
}