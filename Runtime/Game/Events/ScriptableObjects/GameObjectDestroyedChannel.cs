using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/GameObject Destroyed Event Channel")]
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
