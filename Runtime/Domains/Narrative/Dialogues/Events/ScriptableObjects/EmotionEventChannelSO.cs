using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Narrative.Dialogues.Events.ScriptableObjects
{
	using Utilities;
	using StoryTime.Domains.Events.ScriptableObjects;
	
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Emotion Event Channel")]
	public class EmotionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Emotion> OnEventRaised;

		public void RaiseEvent(Emotion emotion) => OnEventRaised?.Invoke(emotion);
	}
}
