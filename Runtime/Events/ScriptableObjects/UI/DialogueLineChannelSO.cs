using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/UI/Dialogue line Channel")]
	public class DialogueLineChannelSO : ScriptableObject
	{
		public UnityAction<Components.IDialogueLine, Components.ScriptableObjects.CharacterSO> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="line"></param>
		/// <param name="character"></param>
		public void RaiseEvent(Components.IDialogueLine line, Components.ScriptableObjects.CharacterSO character) => OnEventRaised?.Invoke(line, character);
	}
}
