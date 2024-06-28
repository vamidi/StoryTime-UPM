

namespace StoryTime.Components.ScriptableObjects
{
	using Database.Binary;

	/// <summary>
	/// Dialogue Holder
	/// these are the base variables that we need to set
	/// up the dialogue system
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public partial class CharacterSO : IBaseTable<CharacterSO>
	{
		public CharacterSO ConvertRow(TableRow row, CharacterSO scriptableObject = null)
			{
				CharacterSO character = scriptableObject ? scriptableObject : CreateInstance<CharacterSO>();

				if (row.Fields.Count == 0)
				{
					return character;
				}

#if !UNITY_EDITOR
			FirebaseInitializer.Fetch((op) =>
			{
				FirebaseConfigSO config = op.Result;
#else
#endif

				character.ID = row.RowId;
				var entryId = (character.ID + 1).ToString();

				if(!character.characterName.IsEmpty) character.characterName.TableEntryReference = entryId;

				foreach (var field in row.Fields)
				{
					if (field.Key.Equals("id"))
					{
						character.ID = (string)field.Value.Data;
					}

					if (field.Key.Equals("initialLevel"))
					{
						character.initialLevel = (int)field.Value.Data;
					}

					if (field.Key.Equals("maxLevel"))
					{
						character.maxLevel = (int)field.Value.Data;
					}
				}

				return character;

#if !UNITY_EDITOR
			});
#endif
			}
	}
}
