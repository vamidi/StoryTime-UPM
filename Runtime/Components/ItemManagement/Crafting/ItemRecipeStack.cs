using System;

namespace StoryTime.Components
{
	[Serializable]
	public class ItemRecipeStack : ItemBaseStack<ScriptableObjects.ItemRecipeSO>
	{
		public ItemRecipeStack() { }
		public ItemRecipeStack(ScriptableObjects.ItemRecipeSO item, int amount) : base(item, amount) { }
	}
}
