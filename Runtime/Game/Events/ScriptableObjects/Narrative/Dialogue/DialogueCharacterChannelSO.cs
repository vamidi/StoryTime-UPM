using UnityEngine.Events;
using UnityEngine;

namespace StoryTime.Events.ScriptableObjects
{
	/// <summary>
	/// This class is used for talk interaction events.
	/// Example: start talking to an actor passed as parameter
	/// </summary>
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Dialogue Character Channel")]
	public class DialogueCharacterChannelSO : ScriptableObject
	{
		public UnityAction<Components.ScriptableObjects.CharacterSO> OnEventRaised;

		public void RaiseEvent(Components.ScriptableObjects.CharacterSO character)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(character);
		}
	}
}
