using UnityEngine.Events;
using UnityEngine;

namespace StoryTime.Events.ScriptableObjects
{
	/// <summary>
	/// This class is used for Events to toggle the interaction UI.
	/// Example: Display or hide the interaction UI via a bool and the interaction type from the enum via int
	/// </summary>
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Toggle Interaction UI Event Channel")]
	public class InteractionUIEventChannelSO : ScriptableObject
	{
		public UnityAction<bool, Components.InteractionType> OnEventRaised;

		public void RaiseEvent(bool state, Components.InteractionType interactionType) => OnEventRaised?.Invoke(state, interactionType);
	}
}

