using UnityEngine;

namespace DatabaseSync.UI
{
	using Components;
	public abstract class InspectorBaseFiller<TInspector, TStack, TItem> : MonoBehaviour
		where TItem: ItemSO
		where TStack: ItemBaseStack<TItem>
		where TInspector : InspectorItemBaseFiller<TStack, TItem>
	{
		[SerializeField] protected TInspector inventoryInspector;

		public virtual void FillItemInspector(TStack itemToInspect, bool[] availabilityArray = null)
		{
			inventoryInspector.gameObject.SetActive(true);
			inventoryInspector.FillItemInspector(itemToInspect, itemToInspect.Item.ItemType.ActionType, availabilityArray);
		}

		public virtual void HideItemInspector()
		{
			inventoryInspector.gameObject.SetActive(false);
		}
	}
}
