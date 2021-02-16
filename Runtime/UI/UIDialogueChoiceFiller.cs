using UnityEngine;

namespace DatabaseSync
{
	using Components;

	public class UIDialogueChoiceFiller : MonoBehaviour
	{
		[SerializeField] private /* LocalizeStringEvent */ DBLocalizeStringEvent choiceText;
		[SerializeField] private DialogueChoiceChannelSO makeAChoiceEvent;

		DialogueChoiceSO m_CurrentChoice;

		public void FillChoice(DialogueChoiceSO choiceToFill)
		{
			m_CurrentChoice = choiceToFill;
			choiceText.StringReference = choiceToFill.Response;
		}

		public void ButtonClicked()
		{
			if (makeAChoiceEvent != null)
				makeAChoiceEvent.RaiseEvent(m_CurrentChoice);
		}
	}
}
