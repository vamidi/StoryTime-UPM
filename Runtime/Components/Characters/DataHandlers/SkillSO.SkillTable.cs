
namespace StoryTime.Components.ScriptableObjects
{
	using Binary;
	using Database;
	using Configurations.ScriptableObjects;

	public partial class SkillSO
	{
		public class SkillTable : BaseTable<SkillSO>
		{
			public new static SkillSO ConvertRow(TableRow row, SkillSO scriptableObject)
			{
				SkillSO skill = scriptableObject ? scriptableObject : CreateInstance<SkillSO>();

				if (row.Fields.Count == 0)
				{
					return skill;
				}

				DatabaseConfigSO config = TableDatabase.Fetch();
				if (config != null)
				{
					skill.ID = row.RowId;
					var entryId = (skill.ID + 1).ToString();

					if(!skill.skillName.IsEmpty) skill.skillName.TableEntryReference = entryId;
					if(!skill.description.IsEmpty) skill.description.TableEntryReference = entryId;
				}

				foreach (var field in row.Fields)
				{
					if (field.Key.Equals("classId"))
					{
						skill.classId = (uint)field.Value.Data;
					}

					if (field.Key.Equals("critical"))
					{
						skill.criticalChance = (bool)field.Value.Data;
					}

					if (field.Key.Equals("magicCost"))
					{
						skill.magicCost = (int)field.Value.Data;
					}

					if (field.Key.Equals("level"))
					{
						skill.level = (int)field.Value.Data;
					}

					if (field.Key.Equals("scope"))
					{
						skill.scope = (int)field.Value.Data;
					}

					if (field.Key.Equals("speed"))
					{
						skill.speed = (float)field.Value.Data;
					}

					if (field.Key.Equals("successRate"))
					{
						skill.successRate = (float)field.Value.Data;
					}

					if (field.Key.Equals("repeat"))
					{
						skill.repeat = (uint)field.Value.Data;
					}

					if (field.Key.Equals("magicCurve"))
					{
						uint paramId = (uint)field.Value.Data;
						var linkField = skill.GetField("attributes", "alias", paramId);
						if (linkField != null)
						{
							skill.magicCurve = linkField.Data;
						}
					}

					if (field.Key.Equals("dmgParameter"))
					{
						uint paramId = (uint)field.Value.Data;
						var linkField = skill.GetField("attributes", "alias", paramId);
						if (linkField != null)
						{
							skill.parameter = linkField.Data;
						}
					}

					if (field.Key.Equals("dmgType"))
					{
						skill.type = (DamageType)field.Value.Data;
					}

					if (field.Key.Equals("formula"))
					{
						skill.formula = (string)field.Value.Data;
					}

					if (field.Key.Equals("variance"))
					{
						skill.variance = (float)field.Value.Data;
					}
				}

				return skill;
			}
		}
	}
}
