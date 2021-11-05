using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	public class GameObjectDestroyedChannel : EventChannelBaseSO
	{
		public UnityAction<GameObject> OnEventRaised;

		public void RaiseEvent(GameObject other)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(other);
		}
	}
}
