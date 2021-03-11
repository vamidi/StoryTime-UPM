using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	public enum Emotion
	{
		Happy,
		Sad,
		Surprised,
		Angry,
		// Extend to your liking.
	}
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Narrative/Emotion Event Channel")]

	public class EmotionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Emotion> OnEventRaised;

		public void RaiseEvent(Emotion emotion) => OnEventRaised?.Invoke(emotion);
	}
}
