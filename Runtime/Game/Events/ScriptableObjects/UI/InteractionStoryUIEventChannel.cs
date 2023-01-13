using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	public struct StoryInfo
	{
		public int Index;
		public Components.ScriptableObjects.StorySO Story;
		public Components.UI.ScriptableObjects.StoryState State;
	}

	/// <summary>
	/// This class is used for Events to toggle the interaction UI.
	/// Example: Display or hide the interaction UI via a bool and the interaction type from the enum via int
	/// </summary>
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Toggle Interaction Story UI Event Channel")]
	public class InteractionStoryUIEventChannel : ScriptableObject
	{
		public UnityAction<bool, StoryInfo, Components.InteractionType> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="state"></param>
		/// <param name="questInfo"></param>
		/// <param name="interactionType"></param>
		public void RaiseEvent(bool state, StoryInfo questInfo, Components.InteractionType interactionType)
			=> OnEventRaised?.Invoke(state, questInfo, interactionType);
	}
}
