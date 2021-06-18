using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Narrative/Dialogue Story Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueStoryChannelSO : ScriptableObject
	{
		public UnityAction<Components.SimpleStorySO> OnEventRaised;
		public void RaiseEvent(Components.SimpleStorySO story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
