using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif


namespace StoryTime.Domains.Game.Characters.Services
{
	using ScriptableObjects;
	using StoryTime.Domains.Database.Binary;

	public class CharacterService
	{
		public CharacterSO ConvertRow(TableRow row, CharacterSO scriptableObject = null)
		{
			CharacterSO character = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<CharacterSO>();

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

			// if(!character.characterName.IsEmpty) character.characterName.TableEntryReference = entryId;

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("id"))
				{
					character.ID = (string)field.Value.Data;
				}

				if (field.Key.Equals("initialLevel"))
				{
					// character.initialLevel = (int)field.Value.Data;
				}

				if (field.Key.Equals("maxLevel"))
				{
					// character.maxLevel = (int)field.Value.Data;
				}
			}

			return character;

#if !UNITY_EDITOR
			});
#endif
		}
		
		public CharacterStats ConvertRow(
			TableRow row,
#if UNITY_EDITOR
			StringTableCollection statCollection,
#endif
			CharacterStats statOverride = null
		)
		{
			CharacterStats stat = statOverride ?? new CharacterStats();
/*
			foreach (var field in row.Fields)
			{
				if (field.Value.Data == null)
					continue;

				if (field.Key.Equals("paramId"))
				{
					stat.paramId = (string)field.Value.Data;
#if UNITY_EDITOR
					// Only get the first dialogue.
					var paramEntryId = (stat.paramId + 1).ToString();
					if (statCollection)
						stat.statName = new LocalizedString
						{
							TableReference = statCollection.TableCollectionNameReference,
							TableEntryReference = paramEntryId
						};
					else
						Debug.LogWarning("Collection not found. Did you create any localization tables for Stats");
#endif

					var linkField = TableDatabase.Get.GetField("attributes", "alias", stat.paramId);
					if (linkField != null)
					{
						// TODO fixme
						stat.attribute = linkField.Data;
					}
				}

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
					stat = new CharacterStats
					{
						attribute = new Attribute
						{
							alias = (AttributeType)field.Value.Data
						}
					};
				}
			}
*/
			return stat;
		}
		
		public CharacterClassSO ConvertRow(TableRow row, CharacterClassSO scriptableObject)
		{
			CharacterClassSO characterClass =
				scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<CharacterClassSO>();

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
					// characterClass.expCurve = (string)field.Value.Data;
				}
			}

			return characterClass;
		}
		
		public SkillSO ConvertRow(TableRow row, SkillSO scriptableObject)
		{
			SkillSO skill = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<SkillSO>();

			if (row.Fields.Count == 0)
			{
				return skill;
			}
			
#if !UNITY_EDITOR
			FirebaseInitializer.Fetch((op) =>
			{
				GlobalSettingsSO config = op.Result;
#else
			// StoryTimeSettingsSO config = CreateInstance<StoryTimeSettingsSO>();
#endif
/*
			if (config != null)
			{
				skill.ID = row.RowId;
				var entryId = (skill.ID + 1).ToString();

				if (!skill.skillName.IsEmpty) skill.skillName.TableEntryReference = entryId;
				if (!skill.description.IsEmpty) skill.description.TableEntryReference = entryId;
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
					string paramId = (string)field.Value.Data;
					var linkField = skill.GetField("attributes", "alias", paramId);
					if (linkField != null)
					{
						// skill.magicCurve = linkField.Data;
					}
				}

				if (field.Key.Equals("dmgParameter"))
				{
					string paramId = (string)field.Value.Data;
					var linkField = skill.GetField("attributes", "alias", paramId);
					if (linkField != null)
					{
						// skill.parameter = linkField.Data;
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
*/

			return skill;

#if !UNITY_EDITOR
			});
#endif
		}
	}
}