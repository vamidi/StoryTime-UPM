using System.Collections.Generic;

using UnityEngine;

namespace DatabaseSync.UI
{
	using Events;
	public class InventoryTypeTabsFiller : MonoBehaviour
	{
		[SerializeField] private List<InventoryTypeTabFiller> tabButtons;

		public InventoryTypeTabFiller selectedTab;
		public List<GameObject> tabAreas;

		public void FillTabs(List<InventoryTabTypeSO> typesList, InventoryTabTypeSO selectedType, TabEventChannelSO changeTabEvent)
		{
			int maxCount = Mathf.Max(typesList.Count, tabButtons.Count);

			for (int i = 0; i < maxCount; i++)
			{
				if (i < typesList.Count)
				{
					if (i >= tabButtons.Count)
					{
						Debug.Log("Maximum tabs reached");
						// TODO create new tabs if the user allows it.
					}
					bool isSelected = typesList[i] == selectedType;

					// fill
					tabButtons[i].FillTab(typesList[i], isSelected, changeTabEvent);
					tabButtons[i].gameObject.SetActive(true);

				}
				else if (i < tabButtons.Count)
				{
					// Deactivate
					tabButtons[i].gameObject.SetActive(false);
				}

			}
		}

		public void Subscribe(InventoryTypeTabFiller button)
		{
			tabButtons.Add(button);
		}

		public void OnTabEnter(InventoryTypeTabFiller button)
		{
			ResetTabs();
		}

		public void OnTabExit(InventoryTypeTabFiller button)
		{
			ResetTabs();
		}

		public void OnTabSelected(InventoryTypeTabFiller button)
		{
			if(selectedTab != null) selectedTab.Deselect();

			selectedTab = button;
			selectedTab.Select();

			ResetTabs();
			// Buttons are sending the events.
			// int index = button.transform.GetSiblingIndex();
			// for (int i = 0; i < tabAreas.Count; i++)
			// 	tabAreas[i].SetActive(i == index);
		}

		public void ResetTabs()
		{
			foreach (var button in tabButtons)
			{
				if (selectedTab != null && button == selectedTab) continue;
				if (button.imgSelected) button.imgSelected.gameObject.SetActive(false);
			}
		}
	}
}
