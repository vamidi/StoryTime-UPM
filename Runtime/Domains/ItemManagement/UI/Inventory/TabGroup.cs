using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StoryTime.Domains.ItemManagement.UI.Inventory
{
	public class TabGroup : MonoBehaviour
	{
		public List<TabButton> tabButtons = new List<TabButton>();
		public TabButton selectedTab;

		public Sprite tabIdle;
		public Sprite tabHover;
		public Sprite tabActive;

		public List<GameObject> tabAreas;
		public void Subscribe(TabButton button)
		{
			tabButtons.Add(button);
		}

		public void OnTabEnter(TabButton button)
		{
			ResetTabs();
			if(selectedTab == null || button != selectedTab)
				button.background.sprite = tabHover;
		}

		public void OnTabExit(TabButton button)
		{
			ResetTabs();
		}

		public void OnTabSelected(TabButton button)
		{
			if(selectedTab != null) selectedTab.Deselect();

			selectedTab = button;
			selectedTab.Select();

			ResetTabs();
			button.background.sprite = tabActive;
			int index = button.transform.GetSiblingIndex();
			for (int i = 0; i < tabAreas.Count; i++)
				tabAreas[i].SetActive(i == index);
		}

		public void ResetTabs()
		{
			foreach (var button in tabButtons)
			{
				if (selectedTab != null && button == selectedTab) continue;
				button.background.sprite = tabIdle;
			}
		}
	}
}
