using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;
	using Events.ScriptableObjects;
	public class RecipeManager : MonoBehaviour
	{
		[SerializeField] protected InventorySO currentInventory;
		[SerializeField] protected RecipeCollectionSO recipeInventory;

		[Header("Listening to channels")]
		[SerializeField] private ItemEventChannelSO craftRecipeEvent;
		[SerializeField] private ItemEventChannelSO cookRecipeEvent;

		[Header("Broadcasting channels")]
		[SerializeField] private VoidEventChannelSO startCrafting;

		[SerializeField] private ItemEventChannelSO addItemEvent;
		[SerializeField] private ItemEventChannelSO removeItemEvent;

		public void InteractWithCharacter()
		{
			startCrafting.RaiseEvent();
		}

		private void OnEnable()
		{
			// Check if the event exists to avoid errors
			if (craftRecipeEvent != null)
			{
				craftRecipeEvent.OnRecipeEventRaised += CraftCookRecipeEventRaised;
			}

			if (cookRecipeEvent != null)
			{
				cookRecipeEvent.OnRecipeEventRaised += CraftCookRecipeEventRaised;
			}
		}

		private void OnDisable()
		{
			if (craftRecipeEvent != null)
			{
				craftRecipeEvent.OnRecipeEventRaised -= CraftCookRecipeEventRaised;
			}

			if (cookRecipeEvent != null)
			{
				cookRecipeEvent.OnRecipeEventRaised -= CraftCookRecipeEventRaised;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="recipe"></param>
		protected virtual void CraftCookRecipeEventRaised(ItemRecipeStack recipe)
		{
			// Find recipe
			if (recipeInventory.Contains(recipe))
			{
				List<ItemStack> ingredients = recipe.Item.IngredientsList;
				// Remove ingredients (when it's a consumable)
				if (recipeInventory.HasIngredients(currentInventory, ingredients))
				{
					foreach (var ingredient in ingredients)
					{
						// if(ingredient.Item.ItemType.ActionType == ItemInventoryActionType.Use)
						// Remove item
						if(removeItemEvent != null) removeItemEvent.RaiseEvent(ingredient);
					}

					// add crafted item or dish
					if(addItemEvent != null) addItemEvent.RaiseEvent(new ItemStack(recipe.Item.ResultingDish, 1));
				}
			}
		}
	}
}
