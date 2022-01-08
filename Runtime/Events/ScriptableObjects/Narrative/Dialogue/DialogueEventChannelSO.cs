using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Narrative/Dialogue Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueEventChannelSO : EventChannelBaseSO
	{
		// TODO maybe make a string to see where the event is coming from.
		public UnityAction<string, System.Object> OnEventRaised;

		public void RaiseEvent(string choiceType, System.Object value) => OnEventRaised?.Invoke(choiceType, value);
	}
}
