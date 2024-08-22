using System.Collections.Generic;
using UnityEngine;

using StoryTime.Domains.ItemManagement.Inventory;
namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects.ItemManagement
{
	public class ItemMasterNode : Node
	{
		public List<IngredientNode> IngredientsList => ingredientsList;

		[SerializeField, Tooltip("The list of the ingredients necessary to the recipe")]
		private List<IngredientNode> ingredientsList = new ();

		public List<ItemStack> GetIngredients()
		{
			List<ItemStack> list = new ();
			foreach (var ingredientNode in ingredientsList)
			{
				list.Add(ingredientNode.Ingredient);
			}

			return list;
		}
	}
}
