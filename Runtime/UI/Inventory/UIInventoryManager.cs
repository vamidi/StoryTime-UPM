using System.Collections.Generic;
using UnityEngine;

namespace DatabaseSync.UI
{
	using Components;
	using Events;

	public class UIInventoryManager : ItemManager<
		InventoryItemListFiller,
		InventoryItemFiller,
		InventoryInspectorFiller,
		InventoryItemInspectorFiller,
		ItemStack,
		ItemSO>
	{
		[Header("Broadcasting channels")] [SerializeField]
		protected ItemEventChannelSO useItemEvent;

		[SerializeField] protected ItemEventChannelSO equipItemEvent;

		protected override void OnEnable()
		{
			base.OnEnable();

			if (changeTabEvent != null)
			{
				changeTabEvent.OnEventRaised += ChangeTabEventRaised;
			}

			if (selectItemEvent != null)
			{
				selectItemEvent.OnItemEventRaised += InspectItem;
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (changeTabEvent != null)
			{
				changeTabEvent.OnEventRaised -= ChangeTabEventRaised;
			}

			if (selectItemEvent != null)
			{
				selectItemEvent.OnItemEventRaised -= InspectItem;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="itemToUse"></param>
		protected virtual void UseItem(ItemStack itemToUse)
		{
			Debug.Log("USE ITEM " + itemToUse.Item.ItemName);

			useItemEvent.OnItemEventRaised(itemToUse);

			// Update inventory
			FillInventory();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="itemToUse"></param>
		protected virtual void EquipItem(ItemStack itemToUse)
		{
			Debug.Log("USE ITEM " + itemToUse.Item.ItemName);
			equipItemEvent.OnItemEventRaised(itemToUse);
		}

		protected override void OnActionButtonRaised(ItemStack itemStack)
		{
			//check the selected Item type
			//call action function depending on the itemType
			switch (itemStack.Item.ItemType.ActionType)
			{
				case ItemInventoryActionType.Use:
					UseItem(itemStack);
					break;
				case ItemInventoryActionType.Equip:
					EquipItem(itemStack);
					break;
			}
		}

		protected override List<ItemStack> FindAll()
		{
			return currentInventory.Items.FindAll(o => o.Item.ItemType.TabType == SelectedTab);
		}

		protected override void ChangeTabEventRaised(InventoryTabTypeSO tabType)
		{
			//
			FillInventory(tabType.TabType);
		}

		protected override void HideItemInformation()
		{
			buttonFiller.gameObject.SetActive(false);
			inspectorFiller.HideItemInspector();
		}

		void ShowItemInformation(ItemStack item)
		{
			inspectorFiller.FillItemInspector(item);
		}

		void InspectItem(ItemStack itemToInspect)
		{
			for (int rowIndex = 0; rowIndex < instantiatedRows.Count; rowIndex++)
			{
				var row = instantiatedRows[rowIndex];
				if (row.SelectItem(SelectedItemId, itemToInspect, out var itemIndex))
				{
					// change Selected ID
					// TODO check if the indices also works for multiple rows.
					SelectedItemId = itemIndex /* + 1 */ + maxItemCapacity * rowIndex;
					// Debug.Log($"UIInventoryManager: {SelectedItemId}");

					// show Information
					ShowItemInformation(itemToInspect);

					// check if interactable
					bool isInteractable = true;
					buttonFiller.gameObject.SetActive(true);
					if (itemToInspect.Item.ItemType.ActionType == ItemInventoryActionType.DoNothing)
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
