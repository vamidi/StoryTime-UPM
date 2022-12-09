using System;

using UnityEngine;

namespace StoryTime.Components
{

	/// <summary>
	/// Drop Item Change Class
	/// </summary>
	[Serializable]
	public class DropItemStack : ItemStack
	{
		public float Weight
		{
			get => weight;
			internal set => weight = value;
		}

		public int MinCountItem
		{
			get => minCountItem;
			internal set => minCountItem = value;
		}
		public int MaxCountItem
		{
			get => maxCountItem;
			internal set => maxCountItem = value;
		}

		[SerializeField, Tooltip("The chance of getting the item dropped.")] private float weight;
		[SerializeField, Tooltip("Minimal amount the player is getting")] private int minCountItem;
		[SerializeField, Tooltip("Maximum amount the player is getting")] private int maxCountItem;

		public DropItemStack() { }

		public DropItemStack(DropItemStack item) : base(item)
		{
			weight = item.weight;
			minCountItem = item.minCountItem;
			maxCountItem = item.maxCountItem;
		}
		public DropItemStack(ScriptableObjects.ItemSO item, int amount) : base(item, amount) { }
	}
}
