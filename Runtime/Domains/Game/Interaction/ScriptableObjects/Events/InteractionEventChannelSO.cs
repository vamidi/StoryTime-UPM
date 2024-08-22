using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
namespace StoryTime.Domains.Game.Interaction.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Interaction/Interaction Event Channel")]
	public class InteractionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<GameObject, Components.InteractionType> OnEventRaised;

		public void RaiseEvent(GameObject other, Components.InteractionType interactionType) => OnEventRaised?.Invoke(other, interactionType);
	}
}
