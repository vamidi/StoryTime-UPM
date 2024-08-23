using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Game.Interaction.UI.Events.ScriptableObjects
{
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
	using StoryTime.Domains.Game.Interaction.UI.Navigation.ScriptableObjects;

	public struct StoryInfo
	{
		public int Index;
		public StorySO Story;
		public StoryState State;
	}

	/// <summary>
	/// This class is used for Events to toggle the interaction UI.
	/// Example: Display or hide the interaction UI via a bool and the interaction type from the enum via int
	/// </summary>
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Toggle Interaction Story UI Event Channel")]
	public class InteractionStoryUIEventChannel : ScriptableObject
	{
		public UnityAction<bool, StoryInfo, InteractionType> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="state"></param>
		/// <param name="questInfo"></param>
		/// <param name="interactionType"></param>
		public void RaiseEvent(bool state, StoryInfo questInfo, InteractionType interactionType)
			=> OnEventRaised?.Invoke(state, questInfo, interactionType);
	}
}
