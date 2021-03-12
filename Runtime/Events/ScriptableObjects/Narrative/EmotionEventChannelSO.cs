using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Narrative/Emotion Event Channel")]

	public class EmotionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.Emotion> OnEventRaised;

		public void RaiseEvent(Components.Emotion emotion) => OnEventRaised?.Invoke(emotion);
	}
}
