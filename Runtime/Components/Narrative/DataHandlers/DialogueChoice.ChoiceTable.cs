using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace StoryTime.Components
{
	using Database.Binary;

	/// <summary>
	/// Dialogue Holder
	/// these are the base variables that we need to set
	/// up the dialogue system
	/// </summary>
	public partial class DialogueChoice
	{
		public class ChoiceTable
		{

			/// <summary>
			/// Converts a row from the json file to a dialogue option.
			/// This can then be immediately use in the game.
			/// </summary>
			/// <param name="row"></param>
			/// <param name="collection"></param>
			/// <param name="option"></param>
			/// <returns>DialogueOption</returns>
			public static DialogueChoice ConvertRow(TableRow row, StringTableCollection collection, DialogueChoice option = null)
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
					if (collection)
						dialogueOption.text = new LocalizedString
							{ TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
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
}
