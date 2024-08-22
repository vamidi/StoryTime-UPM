using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Narrative.Dialogues.Events.UI
{
	using StoryTime.Domains.Events.ScriptableObjects;

	[CreateAssetMenu(menuName = "StoryTime/Game/Events/UI/Dialogue Choice Channel")]
	public class DialogueChoiceChannelSO : EventChannelBaseSO
	{
		public UnityAction<DialogueChoice> OnChoiceEventRaised;
		public UnityAction<List<DialogueChoice>> OnChoicesEventRaised;

		public void RaiseEvent(DialogueChoice choice) => OnChoiceEventRaised?.Invoke(choice);

		public void RaiseEvent(List<DialogueChoice> choices) => OnChoicesEventRaised?.Invoke(choices);
	}
}
