using System;

namespace StoryTime.Domains.ItemManagement.Crafting
{
	using ScriptableObjects;

	[Serializable]
	public class ItemRecipeStack : BaseStack<ItemRecipeSO>
	{
		public ItemRecipeStack() { }
		public ItemRecipeStack(ItemRecipeStack item) : base(item) { }
		public ItemRecipeStack(ItemRecipeSO item, int amount) : base(item, amount) { }
	}
}
