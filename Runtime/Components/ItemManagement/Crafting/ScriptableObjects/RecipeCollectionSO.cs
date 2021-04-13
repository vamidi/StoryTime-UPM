using System.Collections.Generic;

using UnityEngine;

namespace DatabaseSync.Components
{
	[CreateAssetMenu(fileName = "craftingCollection", menuName = "DatabaseSync/Item Management/Crafting or Cooking Collection", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class RecipeCollectionSO : ItemCollection<ItemRecipeStack, ItemRecipeSO>
	{
		public bool[] IngredientsAvailability(InventorySO inventory, List<ItemStack> ingredients)
		{
			bool[] availabilityArray = new bool[ingredients.Count];

			for (int i = 0; i < ingredients.Count; i++)
			{
				availabilityArray[i] = inventory.Items.Exists(o => o.Item == ingredients[i].Item && o.Amount >= ingredients[i].Amount);
			}

			return availabilityArray;
		}

		public bool HasIngredients(InventorySO inventory, List<ItemStack> ingredients)
		{
			bool hasIngredients = !ingredients.Exists(j => !inventory.Items.Exists(o => o.Item == j.Item && o.Amount >= j.Amount));
			return hasIngredients;
		}
	}
}
