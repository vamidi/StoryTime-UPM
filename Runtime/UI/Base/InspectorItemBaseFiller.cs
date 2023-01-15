using UnityEngine;

namespace StoryTime.Components.UI
{
	using Components.ScriptableObjects;

	public abstract class InspectorItemBaseFiller<TStack, TItem>: MonoBehaviour
		where TItem: ItemSO
		where TStack: BaseStack<TItem>
	{
		public abstract void FillItemInspector(TStack itemToInspect, ItemInventoryActionType inventoryActionType,
			bool[] availabilityArray = null);
	}
}
