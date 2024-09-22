using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
using StoryTime.Domains.Narrative.Stories;

namespace StoryTime.Domains.Narrative.Dialogues.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Dialogue Story Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueStoryChannelSO : EventChannelBaseSO
	{
		public UnityAction<IReadOnlyStory> OnEventRaised;
		public void RaiseEvent(IReadOnlyStory story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
