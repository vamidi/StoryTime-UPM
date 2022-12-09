using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Components.UI.ScriptableObjects
{
	public enum StoryState
	{
		New,
		Update,
		Complete
	}

	[CreateAssetMenu(fileName = "InteractionNav", menuName = "StoryTime/Game/UI/Interaction Navigation", order = 51)]
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
