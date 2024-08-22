using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Domains.Narrative.Stories.ScriptableObjects
{
	[CreateAssetMenu(fileName = "storyLog", menuName = "StoryTime/Game/Narrative/Story Log", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class StoryLogSO : ScriptableObject
	{
		public Dictionary<StoryType, List<StorySO>> Stories => GetStories();

		[SerializeField] private List<StoryType> storyCategories = new List<StoryType>();

		[SerializeField] private List<StorySO> stories = new List<StorySO>();

		public void Add(StorySO story)
		{
			if (!storyCategories.Contains(story.TypeId))
			{
				storyCategories.Add(story.TypeId);
				stories.Add(story);
				return;
			}

			if(!Contains(story)) stories.Add(story);
		}

		public void Remove(StorySO story)
		{
			if (!storyCategories.Contains(story.TypeId))
				return;

			if (stories.Count <= 0)
				return;

			foreach (var currentStory in stories)
			{
				if (currentStory == story)
				{
					stories.Remove(currentStory);
					return;
				}
			}
		}

		public bool Contains(StorySO story)
		{
			if (!storyCategories.Contains(story.TypeId))
				return false;

			return stories.Find(s => s == story) != null;
		}

		private Dictionary<StoryType, List<StorySO>> GetStories()
		{
			Dictionary<StoryType, List<StorySO>> currStories = new Dictionary<StoryType, List<StorySO>>();

			if (storyCategories.Count == 0)
				return currStories;

			foreach (var storyCategory in storyCategories)
			{
				if (!currStories.ContainsKey(storyCategory))
					currStories.Add(storyCategory, new List<StorySO>());

				var storyList = currStories[storyCategory];
				foreach (var story in stories)
				{
					if(story.TypeId == storyCategory)
						storyList.Add(story);
				}
			}

			return currStories;
		}
	}
}
