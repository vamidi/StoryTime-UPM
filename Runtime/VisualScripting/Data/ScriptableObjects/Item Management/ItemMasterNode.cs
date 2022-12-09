using System.Collections.Generic;
using StoryTime.Components;
using UnityEngine;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
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
