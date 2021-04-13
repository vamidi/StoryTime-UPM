using UnityEngine;

namespace DatabaseSync.UI
{
	using Components;
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
