using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Game.Characters.ScriptableObjects
{
	using StoryTime.Attributes;
	using StoryTime.Domains.Game.Characters.Services;
	using StoryTime.Domains.Database.ScriptableObjects;
	
	[CreateAssetMenu(fileName = "CharacterClass", menuName = "StoryTime/Game/Characters/Character Class", order = 1)]
	// ReSharper disable once InconsistentNaming
	public class CharacterClassSO : LocalizationBehaviour
	{
		public string ClassName
		{
			get => className;
			internal set => className = value;
		}

		public string ExpCurve
		{
			get => expCurve;
			internal set => expCurve = value;
		}

		public ReadOnlyCollection<SkillSO> Skills => skills.AsReadOnly();
		public ReadOnlyCollection<CharacterStats> Stats => characterStats.AsReadOnly();

		[SerializeField] private string className = "";

		[
			SerializeField,
#if UNITY_EDITOR
			ReadOnly
#endif
		] protected string expCurve;

		[SerializeField] private List<CharacterStats> characterStats;
		[SerializeField] private List<SkillSO> skills;

		private CharacterService characterService = new ();
		
		CharacterClassSO() : base("classes", "className") { }

		protected override void OnTableIDChanged()
		{
			base.OnTableIDChanged();
			Initialize();
		}

		public void OnEnable()
		{
#if UNITY_EDITOR
			Initialize();
#endif
		}

		private void Initialize()
		{
			if (ID != String.Empty)
			{
				// Clear out every stat modifier the player class has.
				ClearModifiers();

				collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Class Names");
				// Only get the first dialogue.
				var entryId = (ID + 1).ToString();
				if(collection)
				{
					LocalizedString className;
					className = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				}
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables for Classes");

				characterStats.Clear();
				var links = FindLinks("parameterCurves", "classId", ID);
				foreach (var link in links)
				{
					var row = link.Item2;
					if (row.Fields.Count == 0)
						continue;

					CharacterStats stat = new CharacterStats
					{
						ID = link.Item1
					};
#if UNITY_EDITOR
					collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Stat Names");
#endif
					stat = characterService.ConvertRow(row, collection, stat);

					characterStats.Add(stat);
				}
			}
		}

		public void AddSkill(SkillSO skill)
		{
			skills.Add(skill);
		}

		public CharacterStats Find(Attribute alias)
		{
			return characterStats.Find((stat) => stat.Attribute == alias);
		}

		private void ClearModifiers()
		{
			Debug.Log(characterStats);
			foreach (var stat in characterStats)
			{
				if (stat.StatModifiers.Count == 0)
					continue;

				stat.Clear();
			}
		}
	}
}
