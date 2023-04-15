
namespace StoryTime.Components.ScriptableObjects
{
	using Database.Binary;

	public partial class CharacterClassSO : IBaseTable<CharacterClassSO>
	{
		public CharacterClassSO ConvertRow(TableRow row, CharacterClassSO scriptableObject)
		{
			CharacterClassSO characterClass =
				scriptableObject ? scriptableObject : CreateInstance<CharacterClassSO>();

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
