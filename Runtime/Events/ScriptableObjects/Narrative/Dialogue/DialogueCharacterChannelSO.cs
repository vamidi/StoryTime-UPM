using UnityEngine.Events;
using UnityEngine;

namespace DatabaseSync.Events
{
	/// <summary>
	/// This class is used for talk interaction events.
	/// Example: start talking to an actor passed as parameter
	/// </summary>
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Narrative/Dialogue Character Channel")]
	public class DialogueCharacterChannelSO : ScriptableObject
	{
		public UnityAction<Components.CharacterSO> OnEventRaised;

		public void RaiseEvent(Components.CharacterSO character)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(character);
		}
	}
}
