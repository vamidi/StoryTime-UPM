
using UnityEngine;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	using Components;

	public class IngredientNode : Node
	{
		public ItemStack Ingredient => ingredient;

		[SerializeField] private ItemStack ingredient;
	}
}
