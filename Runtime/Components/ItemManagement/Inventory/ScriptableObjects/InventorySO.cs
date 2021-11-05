using System;
using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	// Created with collaboration from:
	// https://forum.unity.com/threads/inventory-system.980646/
	[CreateAssetMenu(fileName = "inventory", menuName = "StoryTime/Item Management/Inventory", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class InventorySO : ItemCollection<ItemStack, ItemSO>
	{
		[SerializeField] private int maxItems = 999;

		private int amountItems = 0;

		public override void OnEnable()
		{
			base.OnEnable();
			amountItems = 0;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="item"></param>
		/// <param name="count"></param>
		public void Add(ItemSO item, int count = 1)
		{
			if (count <= 0)
				return;

			Add(new ItemStack(item, count));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="item"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool Contains(ItemSO item, int amount = 1)
		{
			return Contains(new ItemStack(item, amount));
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public override bool AvailabilityCheck(ItemStack item)
		{
			if (maxItems > amountItems)
				return true;

			return base.AvailabilityCheck(item);
		}
	}
}
