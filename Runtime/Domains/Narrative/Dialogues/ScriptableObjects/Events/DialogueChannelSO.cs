using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Narrative.Dialogues.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Dialogue Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueChannelSO : ScriptableObject
	{
		public UnityAction<DialogueLine> OnEventRaised;
		public void RaiseEvent(DialogueLine dialogueLine) => OnEventRaised?.Invoke(dialogueLine);
	}
}
