﻿
// namespace StoryTime.Components.ScriptableObjects
// namespace StoryTime
// {
	using StoryTime.FirebaseService.Database.Binary;

	public partial class CharacterClassSO
	{
		public class ClassTable : BaseTable<CharacterClassSO>
		{
			public new static CharacterClassSO ConvertRow(TableRow row, CharacterClassSO scriptableObject)
			{
				CharacterClassSO characterClass = scriptableObject ? scriptableObject : CreateInstance<CharacterClassSO>();

				if (row.Fields.Count == 0)
				{
					return characterClass;
				}

				foreach (var field in row.Fields)
				{
					if (field.Value.Data == null)
						continue;

					if (field.Key.Equals("expCurve"))
					{
						characterClass.expCurve = (string)field.Value.Data;
					}
				}

				return characterClass;
			}
		}
	}
// }
