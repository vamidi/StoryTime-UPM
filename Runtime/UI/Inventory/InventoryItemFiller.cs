using UnityEngine;

using TMPro;

namespace DatabaseSync.UI
{
	using Events;
	using Components;

	public class InventoryItemFiller : ItemBaseFiller<ItemStack, ItemSO>
	{
		[SerializeField] private TextMeshProUGUI itemCount;

		public override void SetItem(ItemStack itemStack, bool isSelected, ItemEventChannelSO selectItemEvent)
		{
			base.SetItem(itemStack, isSelected, selectItemEvent);

			itemCount.text = itemStack.Amount.ToString();
			OnPointerExit(null);
		}
	}
}
