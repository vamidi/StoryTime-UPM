using System;
using UnityEngine;

namespace StoryTime.Components
{
	using Attributes;
	using ScriptableObjects;

	[Serializable]
	public class ItemBaseStack<T> where T : ItemSO
	{
		[SerializeField] private T item;
		[SerializeField, Tooltip("Amount we are going to give to the player")] private int amount;
		[SerializeField, Tooltip("Enable max amount of items we can have on this item")] private bool setMax;
		[SerializeField, ConditionalField("setMax"), Tooltip("Max amount we can carry of this item")] private int maxAmount;
		public T Item => item;

		public int Amount { get => amount; set => amount = value; }

		public int Max => maxAmount;

		public ItemBaseStack()
		{
			item = default;
			amount = 0;
		}

		public ItemBaseStack(T item, int amount, int maxAmount = 99)
		{
			this.item = item;
			this.amount = amount;
			this.maxAmount = maxAmount;
		}
	}
}
