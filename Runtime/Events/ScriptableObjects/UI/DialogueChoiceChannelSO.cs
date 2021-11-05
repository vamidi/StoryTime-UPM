using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/UI/Dialogue Choice Channel")]
	public class DialogueChoiceChannelSO : ScriptableObject
	{
		public UnityAction<Components.ScriptableObjects.DialogueChoiceSO> OnChoiceEventRaised;
		public UnityAction<List<Components.ScriptableObjects.DialogueChoiceSO>> OnChoicesEventRaised;

		public void RaiseEvent(Components.ScriptableObjects.DialogueChoiceSO choice) => OnChoiceEventRaised?.Invoke(choice);

		public void RaiseEvent(List<Components.ScriptableObjects.DialogueChoiceSO> choices) => OnChoicesEventRaised?.Invoke(choices);
	}
}
