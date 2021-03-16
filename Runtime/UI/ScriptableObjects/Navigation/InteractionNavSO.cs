using UnityEngine;

namespace DatabaseSync.UI
{
	public enum StoryState
	{
		New,
		Update,
		Complete
	}

	// ReSharper disable once InconsistentNaming
	public class InteractionNavSO : InteractionSO
	{
		[Tooltip("The Story title")]
		[SerializeField] public string interactionStoryState;

		[Tooltip("The Story title")]
		[SerializeField] public string interactionStoryTitle;

		[Tooltip("The Task title")]
		[SerializeField] public string interactionTaskDescription;
	}
}
