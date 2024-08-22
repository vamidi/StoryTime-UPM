using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects
{
	public enum ItemInventoryType
	{
		Recipe,
		Utensil,
		Ingredient,
		Customisation,
		Dish,

	}
	public enum ItemInventoryActionType
	{
		DoNothing,
		Cook,
		Craft,
		Use,
		Equip
	}

	[CreateAssetMenu(fileName = "ItemType", menuName = "StoryTime/Game/Item Management/Item Type", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class ItemTypeSO : ScriptableObject
	{
		public LocalizedString ActionName => actionName;
		public LocalizedString TypeName => typeName;
		public Color TypeColor => typeColor;
		public ItemInventoryActionType ActionType => actionType;
		public ItemInventoryType Type => type;
		public InventoryTabTypeSO TabType => tabType;

		[SerializeField, Tooltip("The action associated with the item type")] private LocalizedString actionName;
		[SerializeField, Tooltip("The action associated with the item type")] private LocalizedString typeName;
		[SerializeField, Tooltip("The Item's background color in the UI")] private Color typeColor;
		[SerializeField, Tooltip("The Item's type")] private ItemInventoryType type;
		[SerializeField, Tooltip("The Item's action type")] private ItemInventoryActionType actionType;
		[SerializeField, Tooltip("The tab type under which the item will be added")] private InventoryTabTypeSO tabType;
	}
}
