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
	public class CharacterTable : BaseTable<CharacterSO>
	{
		public new static CharacterSO ConvertRow(TableRow row, CharacterSO scriptableObject)
		{
			CharacterSO character = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<CharacterSO>();

			if (row.Fields.Count == 0)
			{
				return character;
			}

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("id"))
				{
					character.ID = uint.Parse(field.Value.Data);
				}

				if (field.Key.Equals("name"))
				{
					// actor.ActorName = (string) field.Value.Data;
				}
			}

			return character;
		}
	}
}
