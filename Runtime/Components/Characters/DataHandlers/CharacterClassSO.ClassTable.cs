
namespace DatabaseSync.Components
{
	using Binary;
	using Game;

	public partial class CharacterClassSO
	{
		public class ClassTable : BaseTable<CharacterClassSO>
		{
			public new static CharacterClassSO ConvertRow(TableRow row, CharacterClassSO scriptableObject)
			{
				CharacterClassSO characterClass = scriptableObject ? scriptableObject : CreateInstance<CharacterClassSO>();


				return characterClass;
			}

			public static CharacterStats ConvertStatRow(TableRow row)
			{
				CharacterStats stat = new CharacterStats();

				foreach (var field in row.Fields)
				{
					if (field.Value.Data == null)
						continue;

					if (field.Key.Equals("paramFormula"))
					{
						stat.paramFormula = (string)field.Value.Data;
					}

					if (field.Key.Equals("base"))
					{
						stat.baseValue = (float)field.Value.Data;
					}

					if (field.Key.Equals("flat"))
					{
						stat.flat = (float)field.Value.Data;
					}

					if (field.Key.Equals("rate"))
					{
						stat.rate = (float)field.Value.Data;
					}

					if (field.Key.Equals("alias"))
					{
						stat.alias = (string)field.Value.Data;
					}
				}

				return stat;
			}
		}
	}
}
