using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Story Event Channel")]
	public class StoryEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.ScriptableObjects.StorySO> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="story"></param>
		public void RaiseEvent(Components.ScriptableObjects.StorySO story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
