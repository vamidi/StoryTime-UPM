using UnityEngine;

namespace DatabaseSync.UI
{
	using Components;
	public abstract class InspectorItemBaseFiller<TStack, TItem>: MonoBehaviour
		where TItem: ItemSO
		where TStack: ItemBaseStack<TItem>
	{
		public abstract void FillItemInspector(TStack itemToInspect, ItemInventoryActionType inventoryActionType,
			bool[] availabilityArray = null);
	}
}
