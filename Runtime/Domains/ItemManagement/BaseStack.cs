using System;
using UnityEngine;

namespace StoryTime.Domains.ItemManagement
{
	using Utils.Attributes;

	[Serializable]
	public abstract class BaseStack<T> where T : ScriptableObject
	{
		public T Item { get => item; internal set => item = value; }

		public int Amount { get => amount; internal set => amount = value; }
		public int Max => maxAmount;

		[SerializeField] private T item;
		[SerializeField, Tooltip("Amount we are going to give to the player")] protected int amount;
		[SerializeField, Tooltip("Enable max amount of items we can have on this item")] private bool setMax;
		[SerializeField, ConditionalField("setMax"), Tooltip("Max amount we can carry of this item")] private int maxAmount;

		protected BaseStack()
		{
			item = null;
			amount = 1;
		}

		protected BaseStack(BaseStack<T> item)
		{
			this.item = item.item;
			amount = item.amount;
			setMax = item.setMax;
			maxAmount = item.maxAmount;
		}

		protected BaseStack(T item, int amount, int maxAmount = 99)
		{
			this.item = item;
			this.amount = amount;
			this.maxAmount = maxAmount;
		}
	}
}
