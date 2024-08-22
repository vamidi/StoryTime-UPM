using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.ItemManagement.Equipment.ScriptableObjects
{
	using Game.Characters;
	using StoryTime.Domains.Game.Characters.ScriptableObjects;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
	
	public enum EquipmentCategory
	{
		Weapon,
		Shield,
		Head,
		Body,
		Accessory,
	}

	public enum EquipmentType
	{
		None,
		Sword,
		Armor,
		Dagger,
		Axe,
		Bow,
		Gun,
		Spear,
		Necklace,
	}

	public enum EquipmentStatType
	{

	}

	[Serializable]
	public struct EquipmentStat
	{
		public Attribute alias;
		public float flat;
		public StatModType statType;
	}

	[CreateAssetMenu(fileName = "Equipment", menuName = "StoryTime/Game/Item Management/Equipment", order = 50)]
	// ReSharper disable once InconsistentNaming
	public partial class EquipmentSO : ItemSO
	{
		public string EquipmentName
		{
			get => equipmentName;
			internal set => equipmentName = value;
		}

		public EquipmentCategory Category
		{
			get => category;
			internal set => category = value;
		}

		public EquipmentType Type
		{
			get => type;
			internal set => type = value;
		}

		public CharacterClassSO ClassType
		{
			get => classType;
			internal set => classType = value;
		}
		public ReadOnlyCollection<EquipmentStat> Stats => stats.AsReadOnly();

		[SerializeField, Tooltip("The equipment name")] protected string equipmentName = "";
		[SerializeField, Tooltip("In which category this weapon/armor/accessory falls into.")] protected EquipmentCategory category;
		[SerializeField, Tooltip("The type of weapon or armor this equipment is.")] protected EquipmentType type;
		[SerializeField, Tooltip("The class where this equipment belongs to.")] protected CharacterClassSO classType;
		[SerializeField, Tooltip("Which stats this weapon gives")] protected List<EquipmentStat> stats;

		public EquipmentSO() : base("equipments", "name") { }

		public override void OnEnable()
		{
			base.OnEnable();
#if UNITY_EDITOR
			Initialize();
#endif
		}

		public void AddStat(EquipmentStat stat)
		{
			stats.Add(stat);
		}

		protected override void OnTableIDChanged()
		{
			base.OnTableIDChanged();
			Initialize();
		}

		protected override void Initialize()
		{
			base.Initialize();
			// Get the equipment curves
			stats.Clear();
			var links = FindLinks("parameterCurves", "equipmentId", ID);
			foreach (var link in links)
			{
				var row = link.Item2;
				if (row.Fields.Count == 0)
					continue;

				EquipmentStat stat = new EquipmentStat();

				foreach (var field in row.Fields)
				{
					if (field.Value.Data == null)
						continue;

					if (field.Key.Equals("base") || field.Key.Equals("flat"))
					{
						stat.flat = (float)field.Value.Data;
					}

					if (field.Key.Equals("alias"))
					{
						stat.alias = new Attribute
						{
							alias = (AttributeType)field.Value.Data
						};
					}

					if (field.Key.Equals("statType"))
					{
						stat.statType = (StatModType)field.Value.Data;
					}
				}

				AddStat(stat);
			}
		}
	}
}
