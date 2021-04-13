using UnityEngine;
using UnityEngine.Localization.Components;

namespace DatabaseSync.UI
{
	using Events;
	using Components;
	public class RecipeItemFiller : ItemBaseFiller<ItemRecipeStack, ItemRecipeSO>
	{
		[SerializeField] protected LocalizeStringEvent itemDescription;

		public override void SetItem(ItemRecipeStack itemStack, bool isSelected, ItemEventChannelSO selectItemEvent)
		{
			base.SetItem(itemStack, isSelected, selectItemEvent);

			itemName.StringReference = itemStack.Item.ItemName;
			itemDescription.StringReference = itemStack.Item.Description;
		}
	}
}
