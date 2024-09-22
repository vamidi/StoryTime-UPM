using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Game.Interaction.UI.Navigation.ScriptableObjects
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
		[SerializeField] public string interactionStoryState;

		[Tooltip("The Story title")]
		[SerializeField] public string interactionStoryTitle;

		[Tooltip("The Task title")]
		[SerializeField] public string interactionTaskDescription;
	}
}
