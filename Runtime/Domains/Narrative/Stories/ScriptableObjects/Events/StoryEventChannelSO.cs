using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
namespace StoryTime.Domains.Narrative.Stories.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Story Event Channel")]
	public class StoryEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<StorySO> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="story"></param>
		public void RaiseEvent(StorySO story)
		{
			OnEventRaised?.Invoke(story);
		}
	}
}
