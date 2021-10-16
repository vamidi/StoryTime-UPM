using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DatabaseSync.Game;
using UnityEngine;
using UnityEngine.Localization;
using UnityEditor.Localization;

namespace DatabaseSync.Components
{
	/// <summary>
	/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
	/// </summary>
	[CreateAssetMenu(fileName = "newCharacter", menuName = "DatabaseSync/Stories/Character")]
	// ReSharper disable once InconsistentNaming
	public partial class CharacterSO : LocalizationBehaviour
	{
		public LocalizedString CharacterName => characterName;

		public LocalizedString Description => characterDescription;

		public CharacterClassSO CharacterClass => characterClass;

		public ReadOnlyCollection<EquipmentSO> Equipments => equipments.AsReadOnly();

		public int Level => currentLevel;
		public int MaxLevel => maxLevel;

		[SerializeField] private LocalizedString characterName;
		[SerializeField] private LocalizedString characterDescription;
		// TODO see if we need to make an list out of this.
		[SerializeField, Tooltip("All the stats the character currently has.")] protected CharacterClassSO characterClass;

		[Header("Level")]
		[SerializeField] protected int initialLevel = 1;
		[SerializeField, HideInInspector] protected int currentLevel = Int32.MaxValue;
		[SerializeField] protected int maxLevel = 99;

		[SerializeField] protected List<EquipmentSO> equipments = new List<EquipmentSO>();

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

		public void Equip(EquipmentSO equipment)
		{
			// loop through all the stats that the equipment has.
			foreach (var stat in equipment.Stats)
			{
				// find the alias in the character to see if we have an equipment that updates or downgrades the stat.
				var characterStat = characterClass.Find(stat.alias);
				characterStat?.Add(new StatModifier(stat.flat, stat.statType, this));
			}
		}

		CharacterSO() : base("characters", "name") { }

		private void Initialize()
		{
			if (ID != UInt32.MaxValue)
			{
				// Set level to initial level.
				currentLevel = initialLevel;

				var entryId = (ID + 1).ToString();
				collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Characters");
				if(collection)
					characterName = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables");
			}
		}
	}
}
