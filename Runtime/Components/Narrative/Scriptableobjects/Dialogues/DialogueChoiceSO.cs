using System;
// using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace DatabaseSync.Components
{
	using Binary;
	// using ResourceManagement.Util;

	[CreateAssetMenu(fileName = "newDialogueChoice", menuName = "DatabaseSync/Narrative/Dialogue Choice")]
	// ReSharper disable once InconsistentNaming
	public class DialogueChoiceSO : TableBehaviour
	{
		public LocalizedString Sentence => text;

		public IDialogueLine NextDialogue
		{
			get => nextDialogue;
			set
			{
				Debug.Assert(value is DialogueLine || value == null, "Expected to pass in a DialogueLine in class DialogueLine.");
				nextDialogue = (DialogueLine) value;
			}
		}
		public string DialogueChoiceEvent => eventName;
		public ChoiceActionType ActionType => actionType;

		/// <summary>
		/// The text we use to display.
		/// </summary>
		[SerializeField]
		private LocalizedString text;

		// This needs to be calculated
		[SerializeField]
		private DialogueLine nextDialogue;

		[SerializeField]
		private string eventName = String.Empty;

		[SerializeField]
		private ChoiceActionType actionType;

		public DialogueChoiceSO() : base("dialogueOptions", "text") { }

		public override string ToString()
		{
			// public uint NextDialogueID => nextDialogueID;
			// public List<DialogueOption> Choices => choices;
			return $"Choice, Sentence: {text}";
		}

		/// <summary>
		/// Converts a row from the json file to a dialogue option.
		/// This can then be immediately use in the game.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="collection"></param>
		/// <param name="option"></param>
		/// <returns>DialogueOption</returns>
		public static DialogueChoiceSO ConvertRow(TableRow row, StringTableCollection collection, DialogueChoiceSO option = null)
		{
			// Make an empty object.
			DialogueChoiceSO dialogueOption = option ? option : CreateInstance<DialogueChoiceSO>();

			// If we have no fields return the base object.
			if (row.Fields.Count == 0)
			{
				return dialogueOption;
			}

			if (collection != null)
			{
				dialogueOption.ID = row.RowId;
				var entryId = (dialogueOption.ID + 1).ToString();
				if(collection)
					dialogueOption.text = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables");
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
