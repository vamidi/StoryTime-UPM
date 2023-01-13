using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/String Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class NumberEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<int> OnIntEventRaised;

		public void RaiseEvent(int evt) => OnIntEventRaised?.Invoke(evt);
	}
}
