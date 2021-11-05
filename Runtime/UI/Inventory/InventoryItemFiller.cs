using UnityEngine;

using TMPro;

namespace StoryTime.Components.UI
{
	using Components;
	using Components.ScriptableObjects;
	using Events.ScriptableObjects;

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
