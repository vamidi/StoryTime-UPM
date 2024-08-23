using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Game.Interaction.ScriptableObjects.Events
{
	using StoryTime.Domains.Events.ScriptableObjects;
	
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Interaction/Interaction Event Channel")]
	public class InteractionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<GameObject, InteractionType> OnEventRaised;

		public void RaiseEvent(GameObject other, InteractionType interactionType) => OnEventRaised?.Invoke(other, interactionType);
	}
}
