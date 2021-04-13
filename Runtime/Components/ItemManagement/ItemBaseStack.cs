using System;

using UnityEngine;

namespace DatabaseSync.Components
{
	[Serializable]
	public class ItemBaseStack<T> where T : ItemSO
	{
		[SerializeField] private T item;
		[SerializeField] private int amount;

		public T Item => item;

		public int Amount { get => amount; set => amount = value; }

		public ItemBaseStack()
		{
			item = null;
			amount = 0;
		}

		public ItemBaseStack(T item, int amount)
		{
			this.item = item;
			this.amount = amount;
		}
	}
}
