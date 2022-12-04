using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace StoryTime.Components.UI
{
	using Components;
	using Components.ScriptableObjects;
	using Events.ScriptableObjects;

	public abstract class UIItemManager<TListFiller, TItemFiller, TInspectorFiller, TInspector, TStack, TItem, TCollection> : MonoBehaviour
		where TItem: ItemSO
		where TStack: ItemBaseStack<TItem>, new()
		where TInspector: InspectorItemBaseFiller<TStack, TItem>
		where TInspectorFiller : InspectorBaseFiller<TInspector, TStack, TItem>
		where TItemFiller: ItemBaseFiller<TStack, TItem>
		where TListFiller : ListFiller<TItemFiller, TStack, TItem>
		where TCollection: ItemCollection<TStack, TItem>
	{
		public InventoryTabTypeSO SelectedTab
		{
			get => m_SelectedTab;
			protected set => m_SelectedTab = value;
		}

		public int SelectedItemId
		{
			get => m_SelectedItemId;
			protected set => m_SelectedItemId = value;
		}

		[Header("Management Settings")]
		[SerializeField, Tooltip("The maximum items we can store in each row")] protected int maxItemCapacity = 9;
		[SerializeField] protected bool enableRowCreation = true;

		[Header("Inventory Settings")]
		// TODO this needs to represent the crafting/cooking recipes and the inventory.
		[SerializeField, Tooltip("Reference to the current inventory we want to manipulate")]
		protected TCollection currentInventory;

		[Header("Row Settings")]
		[SerializeField] protected List<TListFiller> instantiatedRows = new List<TListFiller>();
		[SerializeField] protected TListFiller itemListFillerPrefab;
		[SerializeField] protected GameObject contentParent;

		[Header("Inspector")]
		[SerializeField] protected TInspectorFiller inspectorFiller;
		[SerializeField] protected InventoryButtonFiller buttonFiller;

		[Header("Tab Settings")] [SerializeField, Tooltip("This script fills all tabs")]
		protected InventoryTypeTabsFiller tabsFiller;

		[SerializeField] protected List<InventoryTabTypeSO> tabTypesList = new List<InventoryTabTypeSO>();
		[SerializeField] protected LocalizeStringEvent currentTabCategory;

		[Header("Listening to channels")]
		[SerializeField] protected TabEventChannelSO changeTabEvent;

		[SerializeField] protected ItemEventChannelSO selectItemEvent;
		[SerializeField] protected VoidEventChannelSO closeInventoryScreenEvent;
		[SerializeField] protected VoidEventChannelSO onInteractionEndedEvent;
		[SerializeField] private VoidEventChannelSO actionButtonClicked;

		private InventoryTabTypeSO m_SelectedTab;
		private int m_SelectedItemId = -1;

		protected virtual void OnEnable()
		{
			// Check if the event exists to avoid errors
			if (actionButtonClicked != null)
			{
				actionButtonClicked.OnEventRaised += ActionButtonEventRaised;
			}

			if (closeInventoryScreenEvent != null)
				closeInventoryScreenEvent.OnEventRaised += HideItemInformation;

			if (tabTypesList.Count > 0) ChangeTabEventRaised(tabTypesList[0]);
		}

		protected virtual void OnDisable()
		{
			if (actionButtonClicked != null)
			{
				actionButtonClicked.OnEventRaised -= ActionButtonEventRaised;
			}
		}

		public void FillInventory(TabType selectedTabType = TabType.None)
		{
			if ((selectedTabType != TabType.None) && (tabTypesList.Exists(o => o.TabType == selectedTabType)))
			{
				m_SelectedTab = tabTypesList.Find(o => o.TabType == selectedTabType);
			}
			else
			{
				if (tabTypesList != null)
				{
					if (tabTypesList.Count > 0)
					{
						m_SelectedTab = tabTypesList[0];
					}
				}
			}


			// if (m_SelectedTab != null)
			{
				FillTypeTabs(tabTypesList, m_SelectedTab);
				List<TStack> listItemsToShow = FindAll();
				FillItems(listItemsToShow);
			}
			// else
			// {
			// Debug.Log("There's no item tab type ");
			// }
		}

		protected abstract List<TStack> FindAll();

		protected abstract void OnActionButtonRaised(TStack itemStack);

		protected virtual void ChangeTabEventRaised(InventoryTabTypeSO tabType)
		{
			//
			currentTabCategory.StringReference = tabType.TabName;
		}

		protected virtual void HideItemInformation() { }

		private void ActionButtonEventRaised()
		{
			if (actionButtonClicked != null)
			{
				if (m_SelectedItemId >= 0 && instantiatedRows.Count > 0)
				{
					// Find the row where the item is stored in
					int rowIndex = m_SelectedItemId > 0 ? (int) Mathf.Floor((float) m_SelectedItemId / maxItemCapacity) - 1 : 0;
					int itemIndex = rowIndex > 0 ? m_SelectedItemId - (maxItemCapacity + 1) : m_SelectedItemId;

					// Find the selected item
					TStack stack = instantiatedRows[rowIndex].GetItem(itemIndex);
					if (stack != null)
					{
						OnActionButtonRaised(stack);
					}
				}
			}
		}

		private void FillTypeTabs(List<InventoryTabTypeSO> typesList, InventoryTabTypeSO selectedType)
		{
			if(tabsFiller)
				tabsFiller.FillTabs(typesList, selectedType, changeTabEvent);
		}

		private void FillItems(List<TStack> listItemsToShow)
		{
			if (instantiatedRows == null) instantiatedRows = new List<TListFiller>();

			// We get the maximum number of all the rows.
			// this way know if we have more items then the instantiated rows and their capacity.
			int maxCount = Mathf.Max(listItemsToShow.Count, instantiatedRows.Count == 0 ? 0 : maxItemCapacity);

			// Loop through the rows that we have
			for (int i = 0; i < maxCount;)
			{
				if (i < listItemsToShow.Count)
				{
					// If we are able to create new row
					// and if we have more items than the rows and their capacity combined.
					if (i >= instantiatedRows.Count && enableRowCreation)
					{
						// instantiate
						TListFiller instantiatedPrefab =
							Instantiate(itemListFillerPrefab, contentParent.transform);
						instantiatedRows.Add(instantiatedPrefab);
					}
					else instantiatedRows[i].gameObject.SetActive(true);

					// fill
					bool isSelected = SelectedItemId == i;
					var row = instantiatedRows[i];
					row.SetMaxItems(maxItemCapacity);
					// search for the max items that we can store
					// index + maximum items =
					var rangeAmount = i + maxItemCapacity <= listItemsToShow.Count
						? maxItemCapacity
						: listItemsToShow.Count;

					row.AddItems(listItemsToShow.GetRange(i, rangeAmount), isSelected, selectItemEvent);
				}
				// hide the other rows if they exists.
				else if (i < instantiatedRows.Count)
				{
					// Deactivate
					instantiatedRows[i].SetInactiveItem();
				}

				i += maxItemCapacity;
			}

			// Hide information at first.
			HideItemInformation();

			// Unselect selected Item
			if (SelectedItemId >= 0)
			{
				UnselectItem(SelectedItemId);
				SelectedItemId = -1;
			}
		}

		private void UnselectItem(int input)
		{
			// find the row the item is stored in
			int rowIndex = (int) Mathf.Floor((float) input / maxItemCapacity);
			// if we have a row index higher than 0 that means the item is in the other rows.
			int itemIndex = rowIndex > 0 ? input - (maxItemCapacity + 1) : input;
			if (instantiatedRows.Count > rowIndex)
			{
				instantiatedRows[rowIndex].UnselectItem(itemIndex);
			}
		}
	}
}
