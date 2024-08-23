

namespace StoryTime.Domains.ItemManagement.UI.Inventory
{
	using StoryTime.Domains.ItemManagement.Inventory;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
	
	public class InventoryInspectorFiller : InspectorBaseFiller<
		InventoryItemInspectorFiller,
		ItemStack,
		ItemSO> { }
}
