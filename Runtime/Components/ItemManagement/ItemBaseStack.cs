using System;
using UnityEngine;

namespace StoryTime.Components
{
	using Utils.Attributes;
	using ScriptableObjects;

	[Serializable]
	public class ItemBaseStack<T> where T : ItemSO
	{
		public T Item { get => item; internal set => item = value; }
		public int Amount { get => amount; internal set => amount = value; }
		public int Max => maxAmount;

		[SerializeField] private T item;
		[SerializeField, Tooltip("Amount we are going to give to the player")] protected int amount;
		[SerializeField, Tooltip("Enable max amount of items we can have on this item")] private bool setMax;
		[SerializeField, ConditionalField("setMax"), Tooltip("Max amount we can carry of this item")] private int maxAmount;

		public ItemBaseStack()
		{
			item = null;
			amount = 1;
		}

		public ItemBaseStack(ItemBaseStack<T> item)
		{
			this.item = item.item;
			amount = item.amount;
			setMax = item.setMax;
			maxAmount = item.maxAmount;
		}

		public ItemBaseStack(T item, int amount, int maxAmount = 99)
		{
			this.item = item;
			this.amount = amount;
			this.maxAmount = maxAmount;
		}
	}
}
