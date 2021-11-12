using System;
using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace StoryTime.Components
{
	using Binary;
	using Database;

	/// <summary>
	/// Dialogue Holder
	/// these are the base variables that we need to set
	/// up the dialogue system
	/// </summary>
	public partial class DialogueLine
	{
		public class DialogueTable
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
				if (dialogueCollection)
					dialogue.sentence = new LocalizedString
						{ TableReference = dialogueCollection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables");


				/*
				TableDatabase database = TableDatabase.Get;
				Tuple<uint, TableRow> link = database.FindLink("characters", "name", characterID);
				if (link != null)
				{
					var field = database.GetField(link.Item2, "name");
					if (field != null) characterName = (string) field.Data;

					Debug.Log(characterName);
				}
				*/

				foreach (var field in row.Fields)
				{
					if (field.Key.Equals("nextId"))
					{
						uint data = (uint)field.Value.Data;
						dialogue.nextDialogueID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
					}

					if (field.Key.Equals("options"))
					{
						// dialogue.sentence = (string) field.Value.Data;
					}

					// if (field.Key.Equals("parentId"))
					// {
					// uint data = (uint) field.Value.Data;
					// dialogue.parentId = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
					// }
				}

				return dialogue;
			}
		}
	}
}
