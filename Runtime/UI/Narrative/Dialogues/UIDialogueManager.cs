using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

namespace StoryTime.Components.UI
{
	using Components;
	using Components.ScriptableObjects;
	using Events.ScriptableObjects;

	public class UIDialogueManager : MonoBehaviour
	{
		[SerializeField] private UnityEvent<string> sentenceEvent;
		[SerializeField] private LocalizeStringEvent sentence;
		[SerializeField] private LocalizeStringEvent actorName;

		[SerializeField] private UIDialogueChoicesManager choicesManager;
		[SerializeField] private DialogueChoiceChannelSO showChoicesEvent;
		[SerializeField] private VoidEventChannelSO hideChoicesEvent;

		private void Start()
		{
			if (showChoicesEvent != null)
				showChoicesEvent.OnChoicesEventRaised += ShowChoices;

			if(hideChoicesEvent != null)
				hideChoicesEvent.OnEventRaised += HideChoices;

			HideChoices();
		}

		public void SetDialogue(DialogueLine dialogueLine)
		{
			// User can optionally show the name of the character.
			if(!dialogueLine.Speaker.CharacterName.IsEmpty) actorName.StringReference = dialogueLine.Speaker.CharacterName;

			if (dialogueLine.isSimplified && sentenceEvent != null)
			{
				sentenceEvent.Invoke(dialogueLine.SimplifiedSentence);
				return;
			}

			// TODO see class LocalizeStringEvent for the reference variables
			sentence.StringReference = dialogueLine.Sentence;
		}

		void ShowChoices(List<DialogueChoice> choices)
		{
			choicesManager.gameObject.SetActive(true);
			choicesManager.FillChoices(choices);
		}
		void HideChoices()
		{
			choicesManager.gameObject.SetActive(false);
		}
	}
}
