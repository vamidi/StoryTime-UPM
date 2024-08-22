using UnityEngine;

using StoryTime.Domains.ItemManagement;
using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
namespace StoryTime.Components.UI
{

	public abstract class InspectorItemBaseFiller<TStack, TItem>: MonoBehaviour
		where TItem: ItemSO
		where TStack: BaseStack<TItem>
	{
		public abstract void FillItemInspector(TStack itemToInspect, ItemInventoryActionType inventoryActionType,
			bool[] availabilityArray = null);
	}
}
