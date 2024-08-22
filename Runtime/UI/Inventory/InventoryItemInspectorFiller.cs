using UnityEngine;

using StoryTime.Domains.ItemManagement.Inventory;
using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
namespace StoryTime.Components.UI
{
	/// <summary>
	/// This is an example inspector class that handles the children
	/// on screen
	/// </summary>
	public class InventoryItemInspectorFiller : InspectorItemBaseFiller<ItemStack, ItemSO>
	{
		[SerializeField] private InspectorDescriptionFiller inspectorDescriptionFiller = default;

		public override void FillItemInspector(ItemStack itemToInspect, ItemInventoryActionType inventoryActionType, bool[] availabilityArray = null)
		{
			inspectorDescriptionFiller.FillDescription(itemToInspect);
		}
	}
}
