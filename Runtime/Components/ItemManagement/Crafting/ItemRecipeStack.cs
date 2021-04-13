using System;

namespace DatabaseSync.Components
{
	[Serializable]
	public class ItemRecipeStack : ItemBaseStack<ItemRecipeSO>
	{
		public ItemRecipeStack() { }
		public ItemRecipeStack(ItemRecipeSO item, int amount) : base(item, amount) { }
	}
}
