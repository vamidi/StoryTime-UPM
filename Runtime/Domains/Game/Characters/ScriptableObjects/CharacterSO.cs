using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.Localization;
using UnityEditor.Localization;

using org.mariuszgromada.math.mxparser;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace StoryTime.Domains.Game.Characters.ScriptableObjects
{
	using StoryTime.Attributes;
	using ItemManagement.Inventory;
	using StoryTime.Domains.Game.Characters.Modifiers;
	using ItemManagement.Equipment.ScriptableObjects;
	using StoryTime.Domains.Database.ScriptableObjects;

	/// <summary>
	/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
	/// </summary>
	[CreateAssetMenu(fileName = "newCharacter", menuName = "StoryTime/Game/Characters/Character", order = 0)]
	// ReSharper disable once InconsistentNaming
	public class CharacterSO : TableBehaviour
	{
		public const int INVENTORY_COLS = 12;
		public const int INVENTORY_ROWS = 6;
		
		public string CharacterName
		{
			get => characterName;
			internal set => characterName = value;
		}

		public string Description
		{
			get => characterDescription;
			internal set => characterDescription = value;
		}

		public CharacterClassSO CharacterClass {
			get => characterClass;
			internal set => characterClass = value;
		}

		public Texture Icon {
			get => icon;
			set => icon = value; 
		}

		public ReadOnlyCollection<EquipmentSO> Equipments => equipments.AsReadOnly();

		public int Level
		{
			get => currentLevel;
			internal set => currentLevel = value;
		}

		public int MaxLevel
		{
			get => maxLevel;
			internal set => maxLevel = value;
		}

		public int CurrentExp => currentExp;
		public int MaxExp => maxExp;
		
		[
#if ODIN_INSPECTOR
			HorizontalGroup("Split", 55, LabelWidth = 150),
			HideLabel, PreviewField(55, ObjectFieldAlignment.Left),
#endif
			SerializeField 
		]
		private Texture icon;

		[Header("Metadata")]
		[
#if ODIN_INSPECTOR
			VerticalGroup("Split/Meta"),
#endif
			SerializeField
		] 
		private string characterName;
		
		[
#if ODIN_INSPECTOR
			VerticalGroup("Split/Meta"),
#endif
			SerializeField
		]
		private string characterDescription;
		// TODO see if we need to make an list out of this.
		[SerializeField, Tooltip("All the stats the character currently has.")] protected CharacterClassSO characterClass;
		
		[Header("Level")]
		[
#if ODIN_INSPECTOR
			VerticalGroup("Split/Meta"),
#endif
			ReadOnly,
			SerializeField
		]
		protected int initialLevel = 1;
		
		[
#if ODIN_INSPECTOR
			VerticalGroup("Split/Meta"),
#endif
			SerializeField
		]
		protected int currentLevel = Int32.MaxValue;
		
		[
#if ODIN_INSPECTOR
			VerticalGroup("Split/Meta"),
#endif
			SerializeField
		]
		protected int maxLevel = 99;

		[Header("EXP")]
		[
#if ODIN_INSPECTOR
			VerticalGroup("Split/Meta"),
#endif
			SerializeField
		]
		protected int currentExp;
		
		[
#if ODIN_INSPECTOR
			VerticalGroup("Split/Meta"),
#endif
			SerializeField
		] 
		protected int maxExp;

#if ODIN_INSPECTOR
		[ShowInInspector,TabGroup("Starting Inventory"),DisableContextMenu]
#endif
		public ItemStack[,] Inventory = new ItemStack[INVENTORY_COLS, INVENTORY_ROWS];
		
#if ODIN_INSPECTOR
		[TabGroup("Starting Stats"), HideLabel]
#endif		
		public CharacterStats Skills = new CharacterStats();

#if ODIN_INSPECTOR
		[HideLabel]
		[TabGroup("Starting Equipment")]
#endif
		public EquipmentSO StartingEquipment;
		
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

		CharacterSO() : base("characters", "name")
		{
			for (int rows = 0; rows < INVENTORY_ROWS; rows++)
			{
				for (int cols = 0; cols < INVENTORY_COLS; cols++)
				{
					if (Inventory[cols, rows] != null) continue;
					
					Inventory[cols, rows] = new ItemStack();
				}
			}
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

			CalculateExp();
		}
	}
}
