using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Dialogue Story Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueStoryChannelSO : ScriptableObject
	{
		public UnityAction<Components.ScriptableObjects.StorySO> OnEventRaised;
		public void RaiseEvent(Components.ScriptableObjects.StorySO story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
