using System;

namespace StoryTime.Components
{
	[Serializable]
	public class ItemStack : ItemBaseStack<ScriptableObjects.ItemSO>
	{
		public ItemStack() { }
		public ItemStack(ScriptableObjects.ItemSO item, int amount) : base(item, amount) { }
	}
}
