using UnityEngine;
using UnityEngine.Localization.Components;

namespace StoryTime.Components.UI
{
	using Events.ScriptableObjects;

	public class UIDialogueChoiceFiller : MonoBehaviour
	{
		[SerializeField] private LocalizeStringEvent choiceText;
		[SerializeField] private DialogueChoiceChannelSO makeAChoiceEvent;

		private DialogueChoice _currentChoice;

		public void FillChoice(DialogueChoice choiceToFill)
		{
			_currentChoice = choiceToFill;
			choiceText.StringReference = choiceToFill.Sentence;
		}

		public void ButtonClicked()
		{
			if (makeAChoiceEvent != null)
				makeAChoiceEvent.RaiseEvent(_currentChoice);
		}
	}
}
