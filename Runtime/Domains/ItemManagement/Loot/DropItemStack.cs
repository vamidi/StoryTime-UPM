using System;

namespace StoryTime.Domains.ItemManagement.Loot
{
	using Inventory.ScriptableObjects;
	
	/// <summary>
	/// Drop Item Change Class
	/// </summary>
	[Serializable]
	public class DropItemStack : DropStack<ItemSO>
	{
		public DropItemStack() { }
		public DropItemStack(DropItemStack item) : base(item) { }
		public DropItemStack(ItemSO item, int amount) : base(item, amount) { }

	}
}
