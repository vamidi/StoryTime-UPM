using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	public class ToggleEventChannelSO<T> : ScriptableObject
	{
		public UnityAction<bool, T> OnEventRaised;

		public void RaiseEvent(T value, bool exit = false)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(exit, value);
		}
	}
}
