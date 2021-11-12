using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Narrative/Dialogue Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueChannelSO : ScriptableObject
	{
		public UnityAction<Components.DialogueLine> OnEventRaised;
		public void RaiseEvent(Components.DialogueLine dialogueLine) => OnEventRaised?.Invoke(dialogueLine);
	}
}
