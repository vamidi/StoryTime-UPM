using TMPro;

using UnityEngine;

namespace DatabaseSync.UI
{
	using Components;

	public class RecipeItemInspectorFiller : InspectorItemBaseFiller<ItemRecipeStack, ItemRecipeSO>
	{
		[SerializeField] private TextMeshProUGUI minAmountItemCreation;
		[SerializeField] private TextMeshProUGUI maxAmountItemCreation;

		[SerializeField] private RecipeIngredientsFiller recipeIngredientsFiller = default;

		public override void FillItemInspector(ItemRecipeStack itemToInspect, ItemInventoryActionType inventoryActionType,
			bool[] availabilityArray = null)
		{
			// bool isCookingInventory = (itemToInspect.Item.ItemType.ActionType == ItemInventoryActionType.Cook);

			recipeIngredientsFiller.FillIngredients(itemToInspect.Item.IngredientsList, availabilityArray);
			recipeIngredientsFiller.gameObject.SetActive(true);

			// ItemRecipeSO
		}
	}
}
