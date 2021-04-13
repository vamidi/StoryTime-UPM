using System;
using System.Collections.Generic;

using UnityEngine;

namespace DatabaseSync.Components
{
	[CreateAssetMenu(fileName = "itemRecipe", menuName = "DatabaseSync/Item Management/Item Recipe", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class ItemRecipeSO : ItemSO
	{
		public List<ItemStack> IngredientsList => ingredientsList;
		public ItemSO ResultingDish => resultingDish;

		// TODO add Recipe functionality
		[SerializeField, Tooltip("The list of the ingredients necessary to the recipe")]
		private List<ItemStack> ingredientsList = new List<ItemStack>();

		[SerializeField, Tooltip("The resulting dish to the recipe")] private ItemSO resultingDish;

		ItemRecipeSO() : base("shopCraftables", "name", "childId", UInt32.MaxValue, "items") { }
	}
}
