using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Stories/Story Event Channel")]
	public class StoryEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.StorySO> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="story"></param>
		public void RaiseEvent(Components.StorySO story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
