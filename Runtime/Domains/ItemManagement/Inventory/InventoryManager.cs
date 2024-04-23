﻿using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;
	using Events.ScriptableObjects;

	public class InventoryManager : MonoBehaviour
	{
		// Events
		[Header("Broadcasting on channels")]
		[SerializeField] private ItemEventChannelSO equipItemEvent;

		[Header("Listening to channels")]
		[SerializeField] private InventorySO currentInventory;
		[SerializeField] private ItemEventChannelSO useItemEvent;
		[SerializeField] private ItemEventChannelSO rewardItemEvent;
		[SerializeField] private ItemEventChannelSO giveItemEvent;
		[SerializeField] private ItemEventChannelSO addItemEvent;
		[SerializeField] private ItemEventChannelSO addItemsEvent;
		[SerializeField] private ItemEventChannelSO removeItemEvent;
		[SerializeField] private ItemEventChannelSO removeItemsEvent;

		private void OnEnable()
		{
			// Check if the event exists to avoid errors
			if (useItemEvent != null)
			{
				useItemEvent.OnItemEventRaised += UseItemEventRaised;
			}

			if (addItemEvent != null)
			{
				addItemEvent.OnItemEventRaised += AddItem;
			}

			if (addItemsEvent != null)
			{
				addItemsEvent.OnItemsEventRaised += AddItems;
			}

			if (removeItemEvent != null)
			{
				removeItemEvent.OnItemEventRaised += RemoveItem;
			}

			if (removeItemsEvent != null)
			{
				removeItemsEvent.OnItemsEventRaised += RemoveItems;
			}

			if (rewardItemEvent != null)
			{
				rewardItemEvent.OnItemEventRaised += AddItem;
			}
			if (giveItemEvent != null)
			{
				giveItemEvent.OnItemEventRaised += RemoveItem;
			}
		}

		private void OnDisable()
		{
			if (useItemEvent != null)
			{
				useItemEvent.OnItemEventRaised -= UseItemEventRaised;
			}
			if (addItemEvent != null)
			{
				addItemEvent.OnItemEventRaised -= AddItem;
			}
			if (removeItemEvent != null)
			{
				removeItemEvent.OnItemEventRaised -= RemoveItem;
			}
		}

		void AddItemWithUIUpdate(ItemSO item)
		{
			currentInventory.Add(item);
			if (currentInventory.Contains(item))
			{
				ItemStack itemToUpdate = currentInventory.Items.Find(o => o.Item == item);
				//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, false);
			}
		}

		void RemoveItemWithUIUpdate(ItemSO item)
		{
			ItemStack itemToUpdate = new ItemStack();

			if (currentInventory.Contains(item))
			{
				itemToUpdate = currentInventory.Items.Find(o => o.Item == item);
			}

			currentInventory.Remove(itemToUpdate.Item);

			bool removeItem = currentInventory.Contains(item);
			//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, removeItem);

		}
		void AddItem(ItemStack item)
		{
			currentInventory.Add(item.Item, item.Amount);
		}

		void AddItems(List<ItemStack> items)
		{
			foreach (var item in items)
			{
				AddItem(item);
			}
		}

		void RemoveItem(ItemStack item)
		{
			currentInventory.Remove(item.Item, item.Amount);
		}

		void RemoveItems(List<ItemStack> items)
		{
			currentInventory.Remove(items);
		}

		public void UseItemEventRaised(ItemStack item)
		{
			RemoveItem(item);
		}
	}
}