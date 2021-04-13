using System;

namespace DatabaseSync.Components
{
	[Serializable]
	public class ItemStack : ItemBaseStack<ItemSO>
	{
		public ItemStack() { }
		public ItemStack(ItemSO item, int amount) : base(item, amount) { }
	}
}
