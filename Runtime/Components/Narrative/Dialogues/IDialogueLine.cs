using System.Collections.Generic;

using UnityEngine.Localization;

namespace StoryTime.Components
{
	using ScriptableObjects;

	public interface IDialogueLine
	{
		uint ID { get; }
		LocalizedString Sentence { get; }
		DialogueEventSO DialogueEvent { get; set; }
		DialogueType DialogueType { get; }
		IDialogueLine NextDialogue { get; set; }
		uint NextDialogueID { get; }
		List<DialogueChoiceSO> Choices { get; }
	}
}
