using UnityEngine;

using StoryTime.Domains.ItemManagement.Inventory;
namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects.ItemManagement
{

	public class IngredientNode : Node
	{
		public ItemStack Ingredient => ingredient;

		[SerializeField] private ItemStack ingredient;
	}
}
