using UnityEngine;
using UnityEngine.Localization.Components;

namespace StoryTime.Domains.ItemManagement.UI.Crafting
{
	using StoryTime.Domains.ItemManagement.Crafting;
	using StoryTime.Domains.ItemManagement.Crafting.ScriptableObjects;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects.Events;
	
	public class RecipeItemFiller : ItemBaseFiller<ItemRecipeStack, ItemRecipeSO>
	{
		[SerializeField] protected LocalizeStringEvent itemDescription;

		public override void SetItem(ItemRecipeStack itemStack, bool isSelected, ItemEventChannelSO selectItemEvent)
		{
			base.SetItem(itemStack, isSelected, selectItemEvent);

			// TODO fixme
			// itemName.StringReference = itemStack.Item.ItemName;
			// itemDescription.StringReference = itemStack.Item.Description;
		}
	}
}
