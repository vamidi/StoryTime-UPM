using System;
using UnityEngine;

namespace StoryTime.Components
{
	[Serializable]
	public class DropStack<T> : BaseStack<T> where T : ScriptableObject
	{
		public float Weight
		{
			get => weight;
			set => weight = value;
		}

		public int MinCountItem
		{
			get => minCountItem;
			set => minCountItem = value;
		}
		public int MaxCountItem
		{
			get => maxCountItem;
			set => maxCountItem = value;
		}

		[SerializeField, Tooltip("The chance of getting the item dropped.")] private float weight;
		[SerializeField, Tooltip("Minimal amount the player is getting")] private int minCountItem;
		[SerializeField, Tooltip("Maximum amount the player is getting")] private int maxCountItem;

		protected DropStack() { }

		protected DropStack(DropStack<T> item) : base(item)
		{
			weight = item.weight;
			minCountItem = item.minCountItem;
			maxCountItem = item.maxCountItem;
		}
		protected DropStack(T item, int amount) : base(item, amount) { }
	}
}
