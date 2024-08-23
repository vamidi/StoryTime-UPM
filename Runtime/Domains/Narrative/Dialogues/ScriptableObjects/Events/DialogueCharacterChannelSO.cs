using UnityEngine.Events;
using UnityEngine;

namespace StoryTime.Domains.Narrative.Dialogues.ScriptableObjects.Events
{
	using StoryTime.Domains.Events.ScriptableObjects;
	using StoryTime.Domains.Game.Characters.ScriptableObjects;

	/// <summary>
	/// This class is used for talk interaction events.
	/// Example: start talking to an actor passed as parameter
	/// </summary>
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Dialogue Character Channel")]
	public class DialogueCharacterChannelSO : EventChannelBaseSO
	{
		public UnityAction<CharacterSO> OnEventRaised;

		public void RaiseEvent(CharacterSO character)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(character);
		}
	}
}
