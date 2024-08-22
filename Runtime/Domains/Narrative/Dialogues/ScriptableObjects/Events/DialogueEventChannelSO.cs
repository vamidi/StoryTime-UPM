using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
namespace StoryTime.Domains.Narrative.Dialogues.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Dialogue Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueEventChannelSO : EventChannelBaseSO
	{
		// TODO maybe make a string to see where the event is coming from.
		public UnityAction<string, System.Object> OnEventRaised;

		public void RaiseEvent(string choiceType, System.Object value) => OnEventRaised?.Invoke(choiceType, value);
	}
}
