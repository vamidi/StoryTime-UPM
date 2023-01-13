using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.Localization;
using UnityEditor.Localization;

using org.mariuszgromada.math.mxparser;

namespace StoryTime.Components.ScriptableObjects
{
	using Attributes;

	/// <summary>
	/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
	/// </summary>
	[CreateAssetMenu(fileName = "newCharacter", menuName = "StoryTime/Game/Characters/Character", order = 0)]
	// ReSharper disable once InconsistentNaming
	public partial class CharacterSO : LocalizationBehaviour
	{
		public LocalizedString CharacterName
		{
			get => characterName;
			internal set => characterName = value;
		}

		public LocalizedString Description => characterDescription;

		public CharacterClassSO CharacterClass => characterClass;

		public ReadOnlyCollection<EquipmentSO> Equipments => equipments.AsReadOnly();

		public int Level => currentLevel;
		public int MaxLevel => maxLevel;

		public int CurrentExp => currentExp;
		public int MaxExp => maxExp;

		[SerializeField] private LocalizedString characterName;
		[SerializeField] private LocalizedString characterDescription;
		// TODO see if we need to make an list out of this.
		[SerializeField, Tooltip("All the stats the character currently has.")] protected CharacterClassSO characterClass;

		[Header("Level")]
		[SerializeField, ReadOnly] protected int initialLevel = 1;
		[SerializeField] protected int currentLevel = Int32.MaxValue;
		[SerializeField] protected int maxLevel = 99;

		[Header("EXP")]
		[SerializeField] protected int currentExp;
		[SerializeField] protected int maxExp;

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

		CharacterSO() : base("characters", "name") { }

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

		/**
		 *
		 *
		 * @return bool
		 */



		/// <summary>
		/// Add exp to the player current exp.
		/// <param name="addition"></param>
		/// <returns>value whether the player is leveling up.</returns>
		/// </summary>
		public bool AddExp(int addition)
		{
			currentExp += addition;
			if (currentExp > maxExp)
			{
				currentLevel++;
				CalculateExp();
				return true;
			}

			return false;
		}

		private void CalculateExp()
		{
			if (characterClass)
			{
				// var exp = characterClass.ExpCurve.Replace("level", currentLevel.ToString());
				Argument currentLevelArg = new Argument("level", currentLevel);
				Expression eh = new Expression(characterClass.ExpCurve, currentLevelArg);

				var mExp = currentLevel != 1 ? (float)eh.calculate() : 0f;
				maxExp = Mathf.CeilToInt(mExp);

				Argument nextLevelArg = new Argument("level", currentLevel + 1 );
				eh = new Expression(characterClass.ExpCurve, nextLevelArg);
				var difference = currentLevel != 1 ? eh.calculate() - maxExp: currentExp;
				// Debug.Log(difference);
			}
		}

		private void Initialize()
		{
			// Set initial level
			currentLevel = initialLevel;

			if (ID != UInt32.MaxValue)
			{
				var entryId = (ID + 1).ToString();
				collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Character Names");
				if(collection)
					characterName = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables");
			}

			CalculateExp();
		}
	}
}
