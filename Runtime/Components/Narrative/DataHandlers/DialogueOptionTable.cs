using System;
using UnityEngine;

namespace DatabaseSync.Components
{
	using Binary;

	public class DialogueOptionTable : BaseTable<DialogueChoiceSO>
	{
		/// <summary>
		/// Converts a row from the json file to a dialogue option.
		/// This can then be immediately use in the game.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="scriptableObject"></param>
		/// <returns>DialogueOption</returns>
		public new static DialogueChoiceSO ConvertRow(TableRow row, DialogueChoiceSO scriptableObject = null)
		{
			// Make an empty object.
			DialogueChoiceSO dialogueOption = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<DialogueChoiceSO>();

			// If we have no fields return the base object.
			if (row.Fields.Count == 0)
			{
				return dialogueOption;
			}

			// Loop through all the fields and acquire the right data
			// TODO make an interop to let users make their own functions
			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("id"))
				{
					dialogueOption.ID = (uint) field.Value.Data;
				}

				if (field.Key.Equals("childId"))
				{
					uint data = (uint) field.Value.Data;
					dialogueOption.ChildId = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}

				if (field.Key.Equals("text"))
				{
					dialogueOption.Text = (string) field.Value.Data;
				}

				if (field.Key.Equals("parentId"))
				{
					uint data = (uint) field.Value.Data;
					dialogueOption.ParentID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}
			}

			return dialogueOption;
		}
	}
}
