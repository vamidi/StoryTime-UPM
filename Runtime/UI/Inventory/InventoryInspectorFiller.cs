
using StoryTime.Domains.ItemManagement.Inventory;
using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;

namespace StoryTime.Components.UI
{
	public class InventoryInspectorFiller : InspectorBaseFiller<
		InventoryItemInspectorFiller,
		ItemStack,
		ItemSO> { }
}
