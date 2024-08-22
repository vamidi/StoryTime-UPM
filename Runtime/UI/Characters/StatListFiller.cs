using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

using StoryTime.Domains.Game.Characters;
using StoryTime.Domains.Game.Characters.ScriptableObjects.Events;

namespace StoryTime.Components.UI
{
	public class StatListFiller : MonoBehaviour
	{
		[SerializeField] private StatItemFiller itemPrefab;
		[SerializeField] private List<StatItemFiller> instantiatedItems;

		public void AddItems(ReadOnlyCollection<CharacterStats> listItemsToShow, bool isSelected, StatEventChannelSO selectItemEvent)
		{
			int maxCount = Mathf.Max(instantiatedItems.Count, listItemsToShow.Count);
			for (int i = 0; i < maxCount; i++)
			{
				// if we don't have enough instantiated then create it for us.
				// only if we are not at max capacity.
				if (i >= instantiatedItems.Count)
				{
					// instantiate
					StatItemFiller instantiatedPrefab = Instantiate(itemPrefab, transform);
					instantiatedItems.Add(instantiatedPrefab);
				}

				// if we have enough instantiated items continue
				// check if we
				if (i < listItemsToShow.Count)
				{
					instantiatedItems[i].SetStat(listItemsToShow[i], isSelected, selectItemEvent);
				}
				else
				{
					// if we empty spaces in the list Deactivate
					instantiatedItems[i].SetInactiveItem();
				}
				instantiatedItems[i].FillStats();
			}
		}

		public CharacterStats GetItem(int index)
		{
			return instantiatedItems[index].currentStats;
		}

		public bool Exists(CharacterStats itemToInspect)
		{
			return instantiatedItems.Exists(o => o.currentStats == itemToInspect);
		}

		/// <summary>
		/// because itemIndex + 1 + maxItemCapacity * rowIndex <see cref="UIStatsManager.InspectItem"/>
		/// </summary>
		public bool SelectItem(int selectedItemId, CharacterStats itemToInspect, out int itemIndex)
		{
			// TODO index and exist can maybe be combined
			itemIndex = -1;
			if (Exists(itemToInspect))
			{
				itemIndex = instantiatedItems.FindIndex(o => o.currentStats == itemToInspect);

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
