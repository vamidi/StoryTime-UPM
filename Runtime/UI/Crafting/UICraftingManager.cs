using System.Collections.Generic;
using UnityEngine;

namespace DatabaseSync.UI
{
	using Events;
	using Components;
	/// <summary>
	///
	/// </summary>
	public class UICraftingManager : ItemManager<
		RecipeItemListFiller,
		RecipeItemFiller,
		RecipeInspectorFiller,
		RecipeItemInspectorFiller,
		ItemRecipeStack,
		ItemRecipeSO>
	{
		public RecipeCollectionSO SetInventory
		{
			set => m_RecipeInventory = value;
		}
		[Header("Listening to channels")]
		[SerializeField] protected VoidEventChannelSO onIncreaseAmountEvent;

		[Header("Broadcasting channels")]
		[SerializeField] protected ItemEventChannelSO craftRecipeEvent;
		[SerializeField] protected ItemEventChannelSO cookRecipeEvent;

		private RecipeCollectionSO m_RecipeInventory;

		protected override void OnEnable()
		{
			base.OnEnable();

			if (changeTabEvent != null)
			{
				changeTabEvent.OnEventRaised += ChangeTabEventRaised;
			}

			if (selectItemEvent != null)
			{
				selectItemEvent.OnRecipeEventRaised += InspectItem;
			}
		}

		protected override List<ItemRecipeStack> FindAll()
		{
			return m_RecipeInventory.Items.FindAll(o => o.Item.ItemType.TabType == SelectedTab);
		}

		protected override void OnActionButtonRaised(ItemRecipeStack itemStack)
		{
			// Check the selected Item type
			// Call action function depending on the itemType
			switch (itemStack.Item.ItemType.ActionType)
			{
				case ItemInventoryActionType.Cook:
					CookRecipe(itemStack);
					break;
				case ItemInventoryActionType.Craft:
					CraftRecipe(itemStack);
					break;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="recipeToCook"></param>
		protected virtual void CookRecipe(ItemRecipeStack recipeToCook)
		{
			// get item
			cookRecipeEvent.OnRecipeEventRaised(recipeToCook);

			// Update inspector
			InspectItem(recipeToCook);

			// Update inventory
			FillInventory();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="recipeToCraft"></param>
		protected virtual void CraftRecipe(ItemRecipeStack recipeToCraft)
		{
			Debug.Log(craftRecipeEvent);
			// get item
			craftRecipeEvent.OnRecipeEventRaised(recipeToCraft);

			// Update inspector
			InspectItem(recipeToCraft);

			// Update inventory
			FillInventory();
		}

		void ShowItemInformation(ItemRecipeStack item)
		{
			bool[] availabilityArray = m_RecipeInventory.IngredientsAvailability(currentInventory, item.Item.IngredientsList);
			inspectorFiller.FillItemInspector(item, availabilityArray);
		}

		void InspectItem(ItemRecipeStack itemToInspect)
		{
			for (int rowIndex = 0; rowIndex < instantiatedRows.Count; rowIndex++)
			{
				var row = instantiatedRows[rowIndex];
				if (row.SelectItem(SelectedItemId, itemToInspect, out var itemIndex))
				{
					// change Selected ID
					SelectedItemId = itemIndex + 1 + maxItemCapacity * rowIndex;

					// show Information
					ShowItemInformation(itemToInspect);

					// check if interactable
					bool isInteractable = true;
					buttonFiller.gameObject.SetActive(true);
					if (itemToInspect.Item.ItemType.ActionType == ItemInventoryActionType.Craft)
						isInteractable = m_RecipeInventory.HasIngredients(currentInventory, itemToInspect.Item.IngredientsList);
					else if (itemToInspect.Item.ItemType.ActionType == ItemInventoryActionType.DoNothing)
					{
						isInteractable = false;
						buttonFiller.gameObject.SetActive(false);
					}

					// Set button
					buttonFiller.FillInventoryButtons(itemToInspect.Item.ItemType, isInteractable);
					break;
				}
			}
		}
	}
}
