namespace StoryTime.Components
{
	public class ItemStack : ItemBaseStack<ScriptableObjects.ItemSO>
	{
		public ItemStack() { }
		public ItemStack(ItemStack item) : base(item) { }
		public ItemStack(ScriptableObjects.ItemSO item, int amount) : base(item, amount) { }
	}
}
