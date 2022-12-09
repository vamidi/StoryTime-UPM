using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Components.ScriptableObjects
{
	public enum TabType
	{
		None,
		Customization, // Character customization
		Artifacts, // Accessories to boost the character
		Upgrades, // Upgrade materials to level up items.
		FoodItems, // Items that can be consumed
		Recipes, // Crafting or cooking recipes
		Materials, // Gadgets that can be used to activate something
		Stories, // Items that the player needs to use.
	}
	[CreateAssetMenu(fileName = "tabType", menuName = "StoryTime/Game/Item Management/Tab Type", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class InventoryTabTypeSO : ScriptableObject
	{
		public LocalizedString TabName => tabName;
		public Sprite TabIcon => tabIcon;
		public TabType TabType => tabType;

		[SerializeField, Tooltip("The tab Name that will be displayed in the inventory")] private LocalizedString tabName;
		[SerializeField, Tooltip("The tab Picture that will be displayed in the inventory")] private Sprite tabIcon = default;
		[SerializeField, Tooltip("The tab type used to reference the item")] private TabType tabType;
	}
}
