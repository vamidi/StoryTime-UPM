using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Narrative/Dialogue Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueChannelSO : ScriptableObject
	{
		public UnityAction<Components.IDialogueLine, Components.ScriptableObjects.CharacterSO> OnEventRaised;
		public void RaiseEvent(Components.IDialogueLine dialogueLine, Components.ScriptableObjects.CharacterSO character)
		{
			OnEventRaised?.Invoke(dialogueLine, character);
		}
	}
}
