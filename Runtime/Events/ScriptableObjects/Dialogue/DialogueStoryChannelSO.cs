using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Narrative/Dialogue Story Channel")]
	// ReSharper disable once InconsistentNaming
	public class DialogueStoryChannelSO : ScriptableObject
	{
		public UnityAction<Components.StorySO> OnEventRaised;
		public void RaiseEvent(Components.StorySO story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
