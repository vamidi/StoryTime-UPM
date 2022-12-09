using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;
	public abstract class ItemCollection<TStack, TItem> : ScriptableObject
		where TItem: ItemSO
		where TStack: ItemBaseStack<TItem>, new()
	{
		public List<TStack> Items => items;

		[Tooltip("The collection of items and their quantities.")]
		[SerializeField] protected List<TStack> items = new ();
		[SerializeField] protected List<TStack> defaultItems = new ();

		private void OnEnable()
		{
			Init();
		}

		public bool[] IngredientsAvailability(ItemCollection<TStack, TItem> inventory, List<ItemStack> ingredients)
		{
			bool[] availabilityArray = new bool[ingredients.Count];

			for (int i = 0; i < ingredients.Count; i++)
			{
				availabilityArray[i] = inventory.Items.Exists(o => o.Item == ingredients[i].Item && o.Amount >= ingredients[i].Amount);
			}

			return availabilityArray;
		}

		public bool HasIngredients(ItemCollection<TStack, TItem> inventory, List<ItemStack> ingredients)
		{
			bool hasIngredients = !ingredients.Exists(j => !inventory.Items.Exists(o => o.Item == j.Item && o.Amount >= j.Amount));
			return hasIngredients;
		}

		public virtual bool AvailabilityCheck(TStack item)
		{
			if (item.Amount <= 0)
				return false;

			TStack itemStack = items.Find((stack) => stack == item);

			// When we don't have the item at all.
			if (itemStack == null)
				return true;

			// Add if we have equal or more items available
			return itemStack.Max >= item.Amount;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="item"></param>
		/// <param name="count"></param>
		public virtual void Remove(TItem item, int count = 1)
		{
			if (count <= 0)
				return;

			foreach (var currentItemStack in items)
			{
				if (currentItemStack.Item == item)
				{
					currentItemStack.Amount -= count;

					if (currentItemStack.Amount <= 0)
						items.Remove(currentItemStack);

					return;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="stacks"></param>
		public virtual void Remove(List<TStack> stacks)
		{
			if (stacks.Count == 0)
				return;

			foreach (var stack in stacks)
			{
				Remove(stack.Item, stack.Amount);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool Contains(TStack item)
		{
			foreach (var currentItemStack in items)
			{
				if (item.Item == currentItemStack.Item && currentItemStack.Amount >= item.Amount)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="itemsCollected"></param>
		/// <returns></returns>
		public virtual bool Contains(List<TStack> itemsCollected)
		{
			bool hasAllItems = false;
			foreach (var item in itemsCollected)
			{
				hasAllItems = Contains(item);
			}

			return hasAllItems;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public virtual TStack Get(uint itemID)
		{
			foreach (var currentItemStack in items)
			{
				if (itemID == currentItemStack.Item.ID)
				{
					return currentItemStack;
				}
			}

			return null;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual int Count(TStack item)
		{
			foreach (var currentItemStack in items)
			{
				if (item.Item == currentItemStack.Item)
				{
					return currentItemStack.Amount;
				}
			}

			return 0;
		}

		protected virtual void Init()
		{
			if (items == null)
			{
				items = new List<TStack>();
			}
			items.Clear();
			foreach (TStack stack in defaultItems)
			{
				items.Add( (TStack)Activator.CreateInstance(typeof(TStack), stack));
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="item"></param>
		protected void Add(TStack item)
		{
			if (item.Amount <= 0)
				return;

			foreach (var currentItemStack in items)
			{
				if (item.Item == currentItemStack.Item)
				{
					// only add to the amount if the item is usable
					// if (currentItemStack.Item.ItemType.ActionType == ItemInventoryActionType.Use)
					// {
					// if the addition is higher than the max we take the max.
					currentItemStack.Amount = Mathf.Min(currentItemStack.Max, currentItemStack.Amount + item.Amount);
					// }

					return;
				}
			}

			items.Add(item);
		}
	}
}
