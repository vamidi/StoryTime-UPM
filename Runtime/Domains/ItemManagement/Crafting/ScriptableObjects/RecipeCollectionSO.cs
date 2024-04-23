using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "craftingCollection", menuName = "StoryTime/Game/Item Management/Crafting or Cooking Collection", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class RecipeCollectionSO : ItemCollection<ItemRecipeStack, ItemRecipeSO> { }
}
