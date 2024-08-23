using UnityEngine;

using TMPro;


namespace StoryTime.Domains.ItemManagement.UI.Inventory
{
	using StoryTime.Domains.ItemManagement.Inventory;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects.Events;
	
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
