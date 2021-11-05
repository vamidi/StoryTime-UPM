using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Narrative/Emotion Event Channel")]

	public class EmotionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.Emotion> OnEventRaised;

		public void RaiseEvent(Components.Emotion emotion) => OnEventRaised?.Invoke(emotion);
	}
}
