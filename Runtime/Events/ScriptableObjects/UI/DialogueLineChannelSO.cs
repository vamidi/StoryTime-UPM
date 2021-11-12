using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/UI/Dialogue line Channel")]
	public class DialogueLineChannelSO : ScriptableObject
	{
		public UnityAction<Components.DialogueLine> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="line"></param>
		public void RaiseEvent(Components.DialogueLine line) => OnEventRaised?.Invoke(line);
	}
}
