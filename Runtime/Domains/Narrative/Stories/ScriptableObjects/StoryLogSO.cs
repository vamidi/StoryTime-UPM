using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Domains.Narrative.Stories.ScriptableObjects
{
	[CreateAssetMenu(fileName = "storyLog", menuName = "StoryTime/Game/Narrative/Story Log", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class StoryLogSO : ScriptableObject
	{
		public Dictionary<StoryType, List<IReadOnlyStory>> Stories => GetStories();

		[SerializeField] private List<StoryType> storyCategories = new ();

		private readonly List<IReadOnlyStory> _stories = new ();

		public void Add(IReadOnlyStory story)
		{
			if (!storyCategories.Contains(story.TypeId))
			{
				storyCategories.Add(story.TypeId);
				_stories.Add(story);
				return;
			}

			if(!Contains(story)) _stories.Add(story);
		}

		public void Remove(StorySO story)
		{
			if (!storyCategories.Contains(story.TypeId))
				return;

			if (_stories.Count <= 0)
				return;

			foreach (var currentStory in _stories)
			{
				if (currentStory.ID == story.ID)
				{
					_stories.Remove(currentStory);
					return;
				}
			}
		}

		public bool Contains(IReadOnlyStory story)
		{
			if (!storyCategories.Contains(story.TypeId))
				return false;

			return _stories.Find(s => s == story) != null;
		}

		private Dictionary<StoryType, List<IReadOnlyStory>> GetStories()
		{
			Dictionary<StoryType, List<IReadOnlyStory>> currStories = new Dictionary<StoryType, List<IReadOnlyStory>>();

			if (storyCategories.Count == 0)
				return currStories;

			foreach (var storyCategory in storyCategories)
			{
				if (!currStories.ContainsKey(storyCategory))
					currStories.Add(storyCategory, new ());

				var storyList = currStories[storyCategory];
				foreach (var story in _stories)
				{
					if(story.TypeId == storyCategory)
						storyList.Add(story);
				}
			}

			return currStories;
		}
	}
}
