using System.Collections.Generic;
using System.Collections.ObjectModel;
using DatabaseSync.Game;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync.Components
{
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

	public enum ClassType
	{

	}

	public struct EquipmentStat
	{
		public string Alias;
		public float Flat;
		public StatModType StatType;
	}

	[CreateAssetMenu(fileName = "Equipment", menuName = "DatabaseSync/Item Management/Equipment", order = 50)]
	// ReSharper disable once InconsistentNaming
	public class EquipmentSO : LocalizationBehaviour
	{
		public LocalizedString EquipmentName => equipmentName;
		public EquipmentCategory Category => category;
		public EquipmentType Type => type;
		public ClassType ClassType => classType;
		public ReadOnlyCollection<EquipmentStat> Stats => stats.AsReadOnly();

		[SerializeField, Tooltip("Name of the equipment")] protected LocalizedString equipmentName;
		[SerializeField, Tooltip("In which category this weapon/armor/accessory falls into.")] protected EquipmentCategory category;
		[SerializeField, Tooltip("The type of weapon or armor this equipment is.")] protected EquipmentType type;
		[SerializeField, Tooltip("The class where this equipment belongs to.")] protected ClassType classType;
		[SerializeField, Tooltip("Which stats this weapon gives")] protected List<EquipmentStat> stats;

		public EquipmentSO() : base("equipments", "name") { }

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

		public void Initialize()
		{
			// Get the equipment curves
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
						stat.Flat = (float)field.Value.Data;
					}

					if (field.Key.Equals("alias"))
					{
						stat.Alias = (string)field.Value.Data;
					}
				}

				stats.Add(stat);
			}
		}
	}
}
