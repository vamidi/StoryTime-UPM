using UnityEngine;

using TMPro;

namespace DatabaseSync.UI
{
	using Components;
	public class IngredientFiller : ItemBaseFiller<ItemStack, ItemSO> // We only need the images in our case.
	{
		[SerializeField] protected TextMeshProUGUI itemCount;

		[SerializeField] private Color textColorAvailable;
		[SerializeField] private Color textColorUnavailable;

		public void FillIngredient(ItemStack ingredient, bool isAvailable)
		{
			itemPreviewImage.sprite = ingredient.Item.PreviewImage;

			itemCount.text = ingredient.Amount.ToString();
			itemCount.color = isAvailable ? textColorAvailable : textColorUnavailable;

			itemName.StringReference = ingredient.Item.ItemName;
		}
	}
}
