using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
using StoryTime.Domains.Narrative.Stories.ScriptableObjects.Events;
namespace StoryTime.Components.UI
{
	public class UIStoryManager : MonoBehaviour
	{
		[Header("Listening to channels")]
		[SerializeField, Tooltip("Check for incoming stories")] private StoryEventChannelSO onStoryStartedEvent;

		[Header("Localization")]
		public LocalizedString tabTitle;
		public LocalizedString tabSubTitle;

		[Header("Information references")]
		public LocalizeStringEvent storyTitle;
		public LocalizeStringEvent taskTitle;
		public LocalizeStringEvent taskDescription;

		[Tooltip("List view that we need to add the stories to.")]
		public GameObject listView;
		public GameObject tabAreaView;
		public GameObject panel;

		[Tooltip("The prefab that defines a category in the list view.")]
		public StoryCategoryFiller categoryPrefab;
		public StoryCategoryTab categoryTabPrefab;

		private readonly List<StoryCategoryFiller> m_CategoryItems = new List<StoryCategoryFiller>();

		[SerializeField, Tooltip("This will keep track of our progress in the game")]
		private StoryLogSO storyLog;

		public void Start()
		{
			if (onStoryStartedEvent != null)
			{
				onStoryStartedEvent.OnEventRaised += OnNewStoryAdded;
			}

			// Fill in all the stories that we have so far
			foreach (var row in storyLog.Stories)
			{
				Create(row);
			}
		}

		public void Subscribe(StoryCategoryFiller item)
		{
			m_CategoryItems.Add(item);
		}

		public void OnTabSelected(StoryCategoryTab tab)
		{
			ShowCategoriesByTypeOrAll(tab.type);
		}

		public void OnCategoryRowSelected(InteractionStoryFiller row)
		{
			var story = row.Story;
			var task = story.Tasks.Find(o => o.IsDone == false);

			// set the right information
			storyTitle.StringReference = story ? story.Title : null;
			// TODO see if we need a title
			taskTitle.StringReference = task ? task.Description : null;
			taskDescription.StringReference = task ? task.Description : null;

			if (panel)
				panel.SetActive(true);
		}

		public void ShowCategoriesByTypeOrAll(StoryType storyType = null)
		{
			var type = storyType ?? StoryType.All;
			
			foreach (var category in m_CategoryItems)
			{
				// TODO make multi language
				// tabTitle = category.Category == type ? type.ToString() : tabTitle;

				// Show the category if category is selected or is the category is set to none.
				category.gameObject.SetActive(category.Category == type || type == StoryType.All);
			}
		}
		public void HideCategories()
		{
			foreach (var category in m_CategoryItems)
			{
				category.gameObject.SetActive(false);
			}
		}

		private void OnNewStoryAdded(StorySO story)
		{
			foreach (var filler in m_CategoryItems)
			{
				// add it to the main list as well.
				if (filler.Category == StoryType.All || filler.Category == story.TypeId)
				{
					filler.Add(story);
					return;
				}
			}

			// this means we don't have the category, add it to the list
			Create(new KeyValuePair<StoryType, List<StorySO>>(story.TypeId, new List<StorySO>{ story }));
		}

		private void Create(KeyValuePair<StoryType, List<StorySO>> pair)
		{
			StoryCategoryFiller categoryFiller = Instantiate(categoryPrefab, Vector3.zero, Quaternion.identity);
			categoryFiller.manager = this;

			var trans = categoryFiller.transform;
			trans.SetParent(listView.transform);
			trans.localScale = Vector3.one;

			categoryFiller.FillCategory(pair);
			m_CategoryItems.Add(categoryFiller);

			var categoryTab = Instantiate(categoryTabPrefab, Vector3.zero, Quaternion.identity);
			categoryTab.manager = this;
			categoryTab.type = pair.Key;

			trans = categoryTab.transform;
			trans.SetParent(tabAreaView.transform);
			trans.localScale = Vector3.one;
		}
	}
}
