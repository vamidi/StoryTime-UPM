using System;
using UnityEngine;

namespace DatabaseSync.Components
{
	using Binary;

	/// <summary>
	/// Dialogue Holder
	/// these are the base variables that we need to set
	/// up the dialogue system
	/// </summary>
	public class DialogueTable : BaseTable<DialogueLineSO>
	{
		public new static DialogueLineSO ConvertRow(TableRow row, DialogueLineSO scriptableObject = null)
		{
			DialogueLineSO dialogue = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<DialogueLineSO>();

			if (row.Fields.Count == 0)
			{
				return dialogue;
			}

			foreach (var field in row.Fields)
			{
				Debug.Log(field.Key.ColumnName);
				Debug.Log(field.Value.Data);
				if (field.Key.Equals("id"))
				{
					dialogue.ID = uint.Parse(field.Value.Data);
				}

				if (field.Key.Equals("nextId"))
				{
					uint data = (uint) field.Value.Data;
					dialogue.NextDialogueID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}

				if (field.Key.Equals("text"))
				{
					dialogue.Text = (string) field.Value.Data;
				}

				if (field.Key.Equals("parentId"))
				{
					uint data = (uint) field.Value.Data;
					dialogue.ParentID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}

				if (field.Key.Equals("characterId"))
				{
					uint data = (uint) field.Value.Data;
					dialogue.CharacterID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}
			}

			return dialogue;
		}
	}
}
