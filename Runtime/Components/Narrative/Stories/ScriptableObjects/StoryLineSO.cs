using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "storyLine", menuName = "StoryTime/Stories/Story Line", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class StoryLineSO : TableBehaviour
	{
		public List<StorySO> Stories => stories;

		public bool IsDone => isDone;
		[Tooltip("The collection of Quests composing the Quest line")]
		[SerializeField] private List<StorySO> stories = new List<StorySO>();

		[SerializeField] bool isDone;

		public void FinishQuestLine()
		{
			isDone = true;
		}

		StoryLineSO(): base("questTypes", "name") {}
	}
}
