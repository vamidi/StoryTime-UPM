using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync.UI
{
	public enum StoryState
	{
		New,
		Update,
		Complete
	}

	[CreateAssetMenu(fileName = "InteractionNav", menuName = "DatabaseSync/UI/Interaction Navigation", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class InteractionNavSO : InteractionSO
	{
		[Tooltip("The Story title")]
		[SerializeField] public LocalizedString interactionStoryState;

		[Tooltip("The Story title")]
		[SerializeField] public LocalizedString interactionStoryTitle;

		[Tooltip("The Task title")]
		[SerializeField] public LocalizedString interactionTaskDescription;
	}
}
