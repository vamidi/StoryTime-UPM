using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

using UnityEngine;
using UnityEngine.Localization;

using StoryTime.Attributes;
using StoryTime.Database.ScriptableObjects;


// TODO find out why this namespace cant stay
namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "CharacterClass", menuName = "StoryTime/Game/Characters/Character Class", order = 1)]
	// ReSharper disable once InconsistentNaming
	public partial class CharacterClassSO : LocalizationBehaviour
	{
		public LocalizedString ClassName => className;
		public string ExpCurve => expCurve;
		public ReadOnlyCollection<CharacterStats> Stats => characterStats.AsReadOnly();

		[SerializeField] private LocalizedString className;
		[
			SerializeField,
#if UNITY_EDITOR
			ReadOnly
#endif
		] protected string expCurve;
		[SerializeField] private List<CharacterStats> characterStats;
		[SerializeField] private List<SkillSO> skills;

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
			if (ID != UInt32.MaxValue)
			{
				// Clear out every stat modifier the player class has.
				ClearModifiers();

				collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Class Names");
				// Only get the first dialogue.
				var entryId = (ID + 1).ToString();
				if(collection)
					className = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
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
					stat = CharacterStats.StatTable.ConvertRow(row, collection, stat);

					characterStats.Add(stat);
				}
			}
		}

		public CharacterStats Find(string alias)
		{
			return characterStats.Find((stat) => stat.Alias == alias);
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
