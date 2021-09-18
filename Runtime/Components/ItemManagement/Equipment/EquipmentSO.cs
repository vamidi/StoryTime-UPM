using UnityEngine;

namespace DatabaseSync
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

	[CreateAssetMenu(fileName = "ItemType", menuName = "DatabaseSync/Item Management/Item Type", order = 50)]
	public class EquipmentSO : ScriptableObject
	{
		[SerializeField, Tooltip("In which category this weapon/armor/accessory falls into.")] private EquipmentCategory category;
		[SerializeField, Tooltip("The type of weapon or armor this equipment is.")] private EquipmentType type;
	}
}
