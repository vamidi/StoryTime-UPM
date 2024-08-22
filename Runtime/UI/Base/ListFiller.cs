using System.Collections.Generic;

using UnityEngine;

using StoryTime.Domains.ItemManagement;
using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects.Events;

namespace StoryTime.Components.UI
{
	public class ListFiller<TFiller, TStack, TItem> : MonoBehaviour
		where TItem : ItemSO
		where TStack: BaseStack<TItem>
		where TFiller: ItemBaseFiller<TStack, TItem>
	{
		[SerializeField] private TFiller itemPrefab;
		[SerializeField] private List<TFiller> instantiatedItems;

		private int m_MaxItemCapacity = 9;

		public void SetMaxItems(int maxItemCapacity)
		{
			m_MaxItemCapacity = maxItemCapacity;
		}

		public void AddItems(List<TStack> listItemsToShow, bool isSelected, ItemEventChannelSO selectItemEvent)
		{
			Debug.Assert(listItemsToShow.Count <= m_MaxItemCapacity,
				$"We need lower or equal amount of items for this row. items: {listItemsToShow.Count}, max: {m_MaxItemCapacity}"
			);

			int maxCount = Mathf.Max(instantiatedItems.Count, listItemsToShow.Count);
			for (int i = 0; i < maxCount; i++)
			{
				// if we don't have enough instantiated then create it for us.
				// only if we are not at max capacity.
				if (i >= instantiatedItems.Count)
				{
					//instantiate
					TFiller instantiatedPrefab = Instantiate(itemPrefab, transform);
					instantiatedItems.Add(instantiatedPrefab);
				}

				// if we have enough instantiated items continue
				// check if we
				if (i < listItemsToShow.Count)
				{
					instantiatedItems[i].SetItem(listItemsToShow[i], isSelected, selectItemEvent);
				}
				else
				{
					// if we empty spaces in the list Deactivate
					instantiatedItems[i].SetInactiveItem();
				}
			}
		}

		public TStack GetItem(int index)
		{
			return instantiatedItems[index].currentItem;
		}

		public bool Exists(TStack itemToInspect)
		{
			return instantiatedItems.Exists(o => o.currentItem == itemToInspect);
		}

		/// <summary>
		/// because itemIndex + 1 + maxItemCapacity * rowIndex <see cref="UIInventoryManager.InspectItem"/>
		/// </summary>
		public bool SelectItem(int selectedItemId, TStack itemToInspect, out int itemIndex)
		{
			// TODO index and exist can maybe be combined
			itemIndex = -1;
			if (Exists(itemToInspect))
			{
				itemIndex = instantiatedItems.FindIndex(o => o.currentItem == itemToInspect);

				// unselect selected Item
				// Debug.Log($"ListFiller: {selectedItemId}");
				// Debug.Log($"ListFiller index: {itemIndex}");
				// Debug.Log($"ListFiller: {selectedItemId != itemIndex}");
				if (selectedItemId >= 0 && selectedItemId != itemIndex)
					UnselectItem(selectedItemId);
			}

			return itemIndex >= 0;
		}

		public void UnselectItem(int itemIndex)
		{
			if (instantiatedItems.Count > itemIndex)
			{
				instantiatedItems[itemIndex].UnSelectItem();
			}
		}

		public virtual void SetInactiveItem()
		{
			foreach (var filler in instantiatedItems)
				filler.SetInactiveItem();
			gameObject.SetActive(false);
		}
	}
}
