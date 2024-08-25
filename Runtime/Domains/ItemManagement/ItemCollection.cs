using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace StoryTime.Domains.ItemManagement
{
	using Inventory;
	using Inventory.ScriptableObjects;
	
	public abstract class ItemCollection<TStack, TItem> : ScriptableObject
		where TItem: ItemSO
		where TStack: BaseStack<TItem>, new()
	{
		internal sealed class ItemStackLocation
		{
			public int X;
			public int Y;
			public TStack Stack;
		}
		
		public TStack[,] Items => items;
		
		public List<TStack> AllItems => _oneDimensionalStack.Select(location => location.Stack).ToList();

		[
			Tooltip("The collection of items and their quantities."),
#if ODIN_INSPECTOR
			ShowInInspector,
			TabGroup("Items"),
			DisableContextMenu
#endif
		]
		protected TStack[,] items;
		
		[SerializeField]
		protected List<TStack> defaultItems = new ();

		internal List<ItemStackLocation> _oneDimensionalStack = new ();
		
		private void OnEnable()
		{
			Init();
		}

		public virtual bool AvailabilityCheck(TStack item)
		{
			if (item.Amount <= 0)
				return false;

			TStack itemStack = null;
			foreach (var stack in items)
			{
				if (stack == item)
				{
					itemStack = stack;
					break;
				}
			}

			// When we don't have the item at all.
			if (itemStack == null)
				return true;

			// Add if we have equal or more items available
			return itemStack.Max >= item.Amount;
		}

		public virtual TStack Find(TItem item)
		{
			foreach (var stack in Items)
			{
				if (stack.Item == item)
				{
					return stack;
				}
			}

			return null;
		}
		
        public virtual List<TStack> FindAll(InventoryTabTypeSO selectedTab)
		{
			List<TStack> itemsToShow = new List<TStack>();
			
			foreach (var stack in Items)
			{
				if(stack.Item.ItemType.TabType != selectedTab)
				{
					continue;
				}
				
				itemsToShow.Add(stack);
			}

			return itemsToShow;
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

			for (int x = 0; x < items.GetLength(0); x += 1)
			{
				for (int y = 0; y < items.GetLength(1); y += 1)
				{
					var currentItemStack = items[y, x];
					if (currentItemStack.Item == item)
					{
						currentItemStack.Amount -= count;

						if (currentItemStack.Amount <= 0)
						{
							// remove from the one-dimensional stack
							var location = _oneDimensionalStack.Find(stackLocation => 
								stackLocation.Stack.Item == item && stackLocation.X == x && stackLocation.Y == y
                            );

							if (location != null)
							{
								_oneDimensionalStack.Remove(location);
							}
							
							items[y, x] = new TStack();
						}


						return;
					}
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
		public virtual TStack Get(String itemID)
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
			items ??= new TStack[GetCols(), GetRows()];
			for (int rows = 0; rows < GetRows(); rows++)
			{
				for (int cols = 0; cols < GetCols(); cols++)
				{
					items[cols, rows] ??= new TStack();
				}
			}
			
			_oneDimensionalStack.Clear();
			foreach (TStack stack in defaultItems)
			{
				var itemStack = ((TStack)Activator.CreateInstance(typeof(TStack), stack));
				
				Add(itemStack);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="stack"></param>
		protected void Add(TStack stack)
		{
			if (stack.Amount <= 0)
				return;
			
			// See if we already have the item in the one-dimensional stack.
			ItemStackLocation location = null;
			foreach (var stackLocation in _oneDimensionalStack)
			{
				if (stackLocation.Stack.Item != stack.Item)
				{
					continue;
				}
				
				location = stackLocation;
				break;
			}
			
			if(location != null)
			{
				items[location.Y, location.X].Amount = Mathf.Min(items[location.Y, location.X].Max, items[location.Y, location.X].Amount + stack.Amount);
				return;
			}
			
			for (int x = 0; x < items.GetLength(0); x += 1) {
				for (int y = 0; y < items.GetLength(1); y += 1) {
					var currentItemStack = items[x, y];
					if (stack.Item == currentItemStack.Item)
					{
						// only add to the amount if the item is usable
						// if (currentItemStack.Item.ItemType.ActionType == ItemInventoryActionType.Use)
						// {
						// if the addition is higher than the max we take the max.
						currentItemStack.Amount = Mathf.Min(currentItemStack.Max, currentItemStack.Amount + stack.Amount);
						// }
						
						_oneDimensionalStack.Add(new ItemStackLocation
						{
							X = x,
							Y = y,
							Stack = stack
						});

						return;
					}
				}
			}
		}
		
		protected virtual int GetRows()
		{
			return 11;
		}
		
		protected virtual int GetCols()
		{
			return 9;
		}
	}
}
