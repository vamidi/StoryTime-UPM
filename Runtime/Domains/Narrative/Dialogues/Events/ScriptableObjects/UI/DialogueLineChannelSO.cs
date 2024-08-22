using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Narrative.Dialogues.Events.UI
{
	using StoryTime.Domains.Events.ScriptableObjects;

	[CreateAssetMenu(menuName = "StoryTime/Game/Events/UI/Dialogue line Channel")]
	public class DialogueLineChannelSO : EventChannelBaseSO
	{
		public UnityAction<DialogueLine> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="line"></param>
		public void RaiseEvent(DialogueLine line) => OnEventRaised?.Invoke(line);
	}
}
