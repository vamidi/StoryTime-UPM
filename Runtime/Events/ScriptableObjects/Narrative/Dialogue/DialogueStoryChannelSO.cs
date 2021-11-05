using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Narrative/Dialogue Story Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueStoryChannelSO : ScriptableObject
	{
		public UnityAction<Components.ScriptableObjects.SimpleStorySO> OnEventRaised;
		public void RaiseEvent(Components.ScriptableObjects.SimpleStorySO story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
