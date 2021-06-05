using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Narrative/Dialogue Story Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueStoryChannelSO : ScriptableObject
	{
		public UnityAction<Components.StorySO, Components.IDialogueLine, Components.ActorSO> OnEventRaised;
		public void RaiseEvent(Components.StorySO story, Components.IDialogueLine dialogueLine, Components.ActorSO actor = null)
		{
			OnEventRaised?.Invoke(story, dialogueLine, actor);
		}
	}
}
