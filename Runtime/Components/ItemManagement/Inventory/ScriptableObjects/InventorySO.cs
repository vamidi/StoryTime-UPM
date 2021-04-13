using System.Collections.Generic;

using UnityEngine;

namespace DatabaseSync.Components
{
	// Created with collaboration from:
	// https://forum.unity.com/threads/inventory-system.980646/
	[CreateAssetMenu(fileName = "inventory", menuName = "DatabaseSync/Item Management/Inventory", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class InventorySO : ItemCollection<ItemStack, ItemSO>
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="item"></param>
		/// <param name="count"></param>
		public void Add(ItemSO item, int count = 1)
		{
			if (count <= 0)
				return;

			Add(new ItemStack(item, count), count);
		}

		public bool Contains(ItemSO item, int amount = 1)
		{
			return Contains(new ItemStack(item, amount), amount);
		}
	}
}
