using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Interaction/Interaction Event Channel")]
	public class InteractionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<GameObject, Components.InteractionType> OnEventRaised;

		public void RaiseEvent(GameObject other, Components.InteractionType interactionType) => OnEventRaised?.Invoke(other, interactionType);
	}
}
