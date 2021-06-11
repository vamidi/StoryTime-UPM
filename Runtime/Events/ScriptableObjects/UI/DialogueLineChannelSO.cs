using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/UI/Dialogue line Channel")]
	public class DialogueLineChannelSO : ScriptableObject
	{
		public UnityAction<Components.IDialogueLine, Components.CharacterSO> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="line"></param>
		/// <param name="character"></param>
		public void RaiseEvent(Components.IDialogueLine line, Components.CharacterSO character) => OnEventRaised?.Invoke(line, character);
	}
}
