using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor.Localization;

using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync.Components
{
	using Game;

	[CreateAssetMenu(fileName = "Character Class", menuName = "DatabaseSync/Stats/Character Class", order = 0)]
	// ReSharper disable once InconsistentNaming
	public partial class CharacterClassSO : LocalizationBehaviour
	{
		public LocalizedString ClassName => className;
		public ReadOnlyCollection<CharacterStats> Stats => characterStats.AsReadOnly();

		[SerializeField] private LocalizedString className;
		[SerializeField] private List<CharacterStats> characterStats;
		[SerializeField] private List<SkillSO> skills;

		CharacterClassSO() : base("classes", "className")
		{
		}

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

		public CharacterStats Find(string alias)
		{
			return characterStats.Find((stat) => stat.alias == alias);
		}

		private void Initialize()
		{
			if (ID != UInt32.MaxValue)
			{
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

					CharacterStats stat = ClassTable.ConvertStatRow(row);
					stat.index = link.Item1;

					collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Stat Names");
					// Only get the first dialogue.
					var paramEntryId = (stat.index + 1).ToString();
					if(collection)
						stat.statName = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = paramEntryId };
					else
						Debug.LogWarning("Collection not found. Did you create any localization tables for Stats");

					characterStats.Add(stat);
				}
			}
		}
	}
}
