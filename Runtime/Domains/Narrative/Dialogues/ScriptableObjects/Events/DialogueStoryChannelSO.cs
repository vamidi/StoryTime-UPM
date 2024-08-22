using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
using StoryTime.Domains.Narrative.Stories.ScriptableObjects;

namespace StoryTime.Domains.Narrative.Dialogues.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Dialogue Story Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueStoryChannelSO : EventChannelBaseSO
	{
		public UnityAction<StorySO> OnEventRaised;
		public void RaiseEvent(StorySO story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
