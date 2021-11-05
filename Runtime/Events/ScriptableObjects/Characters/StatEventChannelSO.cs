using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Characters/Stat Event Channel")]
	public class StatEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.CharacterStats> OnEventRaised;

		public void RaiseEvent(Components.CharacterStats stats) => OnEventRaised?.Invoke(stats);

	}
}
