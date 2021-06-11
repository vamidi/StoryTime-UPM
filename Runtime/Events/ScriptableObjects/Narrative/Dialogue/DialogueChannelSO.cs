using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Narrative/Dialogue Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueChannelSO : ScriptableObject
	{
		public UnityAction<Components.IDialogueLine, Components.CharacterSO> OnEventRaised;
		public void RaiseEvent(Components.IDialogueLine dialogueLine, Components.CharacterSO character)
		{
			OnEventRaised?.Invoke(dialogueLine, character);
		}
	}
}
