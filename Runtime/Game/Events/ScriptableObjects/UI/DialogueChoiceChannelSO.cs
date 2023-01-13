using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/UI/Dialogue Choice Channel")]
	public class DialogueChoiceChannelSO : ScriptableObject
	{
		public UnityAction<Components.DialogueChoice> OnChoiceEventRaised;
		public UnityAction<List<Components.DialogueChoice>> OnChoicesEventRaised;

		public void RaiseEvent(Components.DialogueChoice choice) => OnChoiceEventRaised?.Invoke(choice);

		public void RaiseEvent(List<Components.DialogueChoice> choices) => OnChoicesEventRaised?.Invoke(choices);
	}
}
