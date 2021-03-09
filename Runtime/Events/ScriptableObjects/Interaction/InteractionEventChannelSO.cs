using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Interaction/Interaction Event Channel")]
	public class InteractionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<GameObject, InteractionType> OnEventRaised;

		public void RaiseEvent(GameObject other, InteractionType interactionType) => OnEventRaised?.Invoke(other, interactionType);
	}
}
