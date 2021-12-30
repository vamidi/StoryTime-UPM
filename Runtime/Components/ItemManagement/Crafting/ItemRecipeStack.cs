using System;

namespace StoryTime.Components
{
	[Serializable]
	public class ItemRecipeStack : ItemBaseStack<ScriptableObjects.ItemRecipeSO>
	{
		public ItemRecipeStack() { }
		public ItemRecipeStack(ItemRecipeStack item) : base(item) { }
		public ItemRecipeStack(ScriptableObjects.ItemRecipeSO item, int amount) : base(item, amount) { }
	}
}
