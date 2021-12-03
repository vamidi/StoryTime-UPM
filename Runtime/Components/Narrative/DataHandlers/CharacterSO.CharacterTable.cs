
namespace StoryTime.Components.ScriptableObjects
{
	using Binary;
	using Configurations.ScriptableObjects;

	/// <summary>
	/// Dialogue Holder
	/// these are the base variables that we need to set
	/// up the dialogue system
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public partial class CharacterSO
	{
		public class CharacterTable : BaseTable<CharacterSO>
		{
			public new static CharacterSO ConvertRow(TableRow row, CharacterSO scriptableObject)
			{
				CharacterSO character = scriptableObject ? scriptableObject : CreateInstance<CharacterSO>();

				if (row.Fields.Count == 0)
				{
					return character;
				}

				DatabaseConfigSO config = TableBinary.Fetch();
				if (config != null)
				{
					character.ID = row.RowId;
					var entryId = (character.ID + 1).ToString();

					if(!character.characterName.IsEmpty) character.characterName.TableEntryReference = entryId;
				}

				foreach (var field in row.Fields)
				{
					if (field.Key.Equals("id"))
					{
						character.ID = (uint)field.Value.Data;
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
			}
		}
	}
}
