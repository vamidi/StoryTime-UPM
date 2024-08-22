using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;

using StoryTime.Domains.Game.Characters.ScriptableObjects;
using StoryTime.Domains.ItemManagement.Equipment.ScriptableObjects;

namespace StoryTime.Editor.Wizards
{
	public class EquipmentWizard : BaseWizard<EquipmentWizard, EquipmentSO>
	{
		[Header("General Settings")]
		[SerializeField, Tooltip("The equipment name")] protected string equipmentName = "";
		[SerializeField, Tooltip("The equipment name")] protected string description = "";

		[Header("")]
		[SerializeField, Tooltip("In which category this weapon/armor/accessory falls into.")] protected EquipmentCategory category;
		[SerializeField, Tooltip("The type of weapon or armor this equipment is.")] protected EquipmentType type;
		[SerializeField, Tooltip("The class where this equipment belongs to.")] protected CharacterClassSO classType;
		[SerializeField, Tooltip("Is this equipment sellable.")] protected bool sellable;
		[SerializeField, Tooltip("The cost of this equipment.")] protected uint sellValue;


		[Header("Stats")]
		[SerializeField, Tooltip("Which stats this weapon gives")] protected List<EquipmentStat> stats;



		protected override bool Validate()
		{
			return true;
		}

		protected override EquipmentSO Create(string location)
		{
			var equipment = CreateInstance<EquipmentSO>();
			equipment.EquipmentName = equipmentName;
			equipment.Description = description;
			equipment.Category = category;
			equipment.Type = type;
			equipment.ClassType = classType;
			equipment.Sellable = sellable;
			equipment.SellValue = sellValue;

			foreach (var stat in stats)
			{
				equipment.AddStat(stat);
			}

			return equipment;
		}
	}
}
