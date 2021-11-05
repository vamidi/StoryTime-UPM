using UnityEngine;
using UnityEngine.Localization.Components;

namespace StoryTime.Components.UI
{
	using Components.ScriptableObjects;
	using Events.ScriptableObjects;

	public class UIDialogueChoiceFiller : MonoBehaviour
	{
		[SerializeField] private LocalizeStringEvent choiceText;
		[SerializeField] private DialogueChoiceChannelSO makeAChoiceEvent;

		DialogueChoiceSO m_CurrentChoice;

		public void FillChoice(DialogueChoiceSO choiceToFill)
		{
			m_CurrentChoice = choiceToFill;
			choiceText.StringReference = choiceToFill.Sentence;
		}

		public void ButtonClicked()
		{
			if (makeAChoiceEvent != null)
				makeAChoiceEvent.RaiseEvent(m_CurrentChoice);
		}
	}
}
