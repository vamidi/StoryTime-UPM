using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Cinemachine;

namespace DatabaseSync.Components
{
	using Input;

	/**
	 *
	 */
	public class DialogueManager: MonoBehaviour
	{
		[SerializeField] private InputReader inputReader;

		private int m_Counter;

		private bool ReachedEndOfDialogue => m_Counter >= m_CurrentStory.DialogueLines.Count;

		[Header("Cameras")]
		public GameObject gameCam;
		public GameObject dialogueCam;

		[Header("Targets")]
		public CinemachineTargetGroup targetGroup;

		[Header("Quests")]
		[SerializeField] private List<QuestSO> quests;

		[Header("Listening on channels")]
		[SerializeField] private DialogueStoryChannelSO startDialogue;
		[SerializeField] private DialogueChoiceChannelSO makeDialogueChoiceEvent;

		[SerializeField] private TransformEventChannelSO startTransformDialogue;

		[Header("BroadCasting on channels")]
		[SerializeField] private DialogueLineChannelSO openUIDialogueEvent;
		[SerializeField] private DialogueChoiceChannelSO showChoicesUIEvent;
		[SerializeField] private DialogueStoryChannelSO endDialogue;
		[SerializeField] private VoidEventChannelSO continueWithTask;
		[SerializeField] private VoidEventChannelSO closeDialogueUIEvent;
		[SerializeField] private VoidEventChannelSO closeChoiceUIEvent;

		[SerializeField] private TextRevealer revealer;

		private StorySO m_CurrentStory;

		private bool _optionShown;
		private bool _interacting;
		private bool _isInputEnabled;

		void Start()
		{
			if (startDialogue != null)
			{
				startDialogue.OnEventRaised += Interact;
			}

			if (startTransformDialogue != null) // Set the target to the person we are talking to.
				startTransformDialogue.OnEventRaised += (tr) => targetGroup.m_Targets[1].target = tr;

			if(gameCam == null)
				gameCam = GameObject.FindWithTag("GameCamera");

			if(dialogueCam == null)
				dialogueCam = GameObject.FindWithTag("DialogueCamera");

			var virtualCam = dialogueCam.GetComponent<CinemachineVirtualCamera>();
			virtualCam.Follow = virtualCam.LookAt = targetGroup.transform;

			ToggleCameras(false);

			var player = GameObject.FindWithTag("Player");
			if(player)
			{
				targetGroup.m_Targets[0].target = player.transform;
			}
		}

		/// <summary>
		/// Displays DialogueData in the UI, one by one.
		/// Start interaction with the NPC
		/// </summary>
		/// <param name="dialogueDataSo"></param>
		public void Interact(StorySO dialogueDataSo)
		{
			BeginDialogueStory(dialogueDataSo);
			ShowDialogue(m_CurrentStory.DialogueLines[m_Counter], dialogueDataSo.Actor);
			ToggleCameras(true);
		}

		/// <summary>
		/// Prepare DialogueManager when first time displaying DialogueData.
		/// <param name="dialogueDataSo"></param>
		/// </summary>
		private void BeginDialogueStory(StorySO dialogueDataSo)
		{
			m_Counter = 0;
			inputReader.EnableDialogueInput();
			inputReader.advanceDialogueEvent += OnAdvance;
			m_CurrentStory = dialogueDataSo;
			_isInputEnabled = false;
			StopAllCoroutines();
		}

		/// <summary>
		/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
		/// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
		/// </summary>
		/// <param name="dialogueLine"></param>
		/// <param name="actor"></param>
		public void ShowDialogue(DialogueLineSO dialogueLine, ActorSO actor)
		{
			if (openUIDialogueEvent != null)
			{
				// send event out before the dialogue starts
				// InitEvents();
				// CallEvents(true);

				openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
			}
			revealer.RevealNextParagraphAsync();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="nextDialogueLineSo"></param>
		public void ShowNextDialogue(DialogueLineSO nextDialogueLineSo)
		{
			// TODO make this work with increment only instead of setting the next dialogue.
			// increment to the next dialogue sequence
			Increment();
			DialogueChoiceEndAndCloseUI();
			ShowDialogue(nextDialogueLineSo, m_CurrentStory.Actor);
		}

		/// <summary>
		/// Show the full dialogue
		/// </summary>
		private void Skip()
		{
			// This means we are already showing text
			revealer.ShowEverythingWithoutAnimation();
		}

		/// <summary>
		/// Show the next dialogue
		/// </summary>
		private void OnAdvance()
		{
			if (revealer.IsRevealing)
			{
				Skip();
				return;
			}

			// if we reached the end of the dialogue then close everything.
			if (!ReachedEndOfDialogue)
			{
				// if not check for dialogue choices
				var currentDialogue = m_CurrentStory.DialogueLines[m_Counter];
				if (currentDialogue.Choices.Count > 0)
				{
					Debug.Log("Choices");
					DisplayChoices(currentDialogue.Choices);
					return;
				}

				// Hide the option when advancing. // TODO this will be helpful for later when we use index instead of next dialogue
				DialogueChoiceEndAndCloseUI();

				Increment();
				currentDialogue = m_CurrentStory.DialogueLines[m_Counter];
				ShowDialogue(currentDialogue, currentDialogue.Actor);
			}
			else
			{
				StartCoroutine(DialogueEndedAndCloseDialogueUI());
			}

			/*
			// Fetch the events
			InitEvents();
			// Call exit events
			CallEvents(false);
			m_CurrentDialogue = Advance();
			if (m_CurrentDialogue != null)
			{
				// reveal the current dialogue text.
				_textRevealer.RestartWithText(m_CurrentDialogue.Text);
				_textRevealer.RevealNextParagraphAsync();
				return;
			}

			HideDialogue();
			*/
		}

		private void DisplayChoices(List<DialogueChoiceSO> choices)
		{
			inputReader.advanceDialogueEvent -= OnAdvance;
			if (makeDialogueChoiceEvent != null)
			{
				makeDialogueChoiceEvent.OnChoiceEventRaised += MakeDialogueChoice;
			}

			if (showChoicesUIEvent != null)
			{
				showChoicesUIEvent.RaiseEvent(choices);
			}
		}

		private void MakeDialogueChoice(DialogueChoiceSO choice)
		{
			if (makeDialogueChoiceEvent != null)
			{
				makeDialogueChoiceEvent.OnChoiceEventRaised -= MakeDialogueChoice;
			}

			if (choice.ActionType == ChoiceActionType.ContinueWithQuest && choice.OptionEvent != null)
			{
				var choiceEvent = choice.OptionEvent;
				// grab the quest from the list
				if (choiceEvent.EventName == "AcceptQuest")
				{
					var quest = quests.Find(q => q.ID == choiceEvent.Value);
					choice.OptionEvent.RaiseEvent(quest);
				}
			}
			else if (choice.ActionType == ChoiceActionType.ContinueWithTask)
			{
				if (continueWithTask != null)
					continueWithTask.RaiseEvent();
				if (choice.NextDialogue != null)
					ShowNextDialogue(choice.NextDialogue);
			}
			else
			{
				if (choice.NextDialogue != null)
					ShowNextDialogue(choice.NextDialogue);
				else
					StartCoroutine(DialogueEndedAndCloseDialogueUI());
			}
		}

		private void DialogueChoiceEndAndCloseUI()
		{
			if (closeChoiceUIEvent != null)
				closeChoiceUIEvent.RaiseEvent();

			inputReader.advanceDialogueEvent += OnAdvance;
		}

		private IEnumerator DialogueEndedAndCloseDialogueUI()
		{
			if (_isInputEnabled) yield break;

			yield return new WaitForSeconds(0.5f);

			if (closeDialogueUIEvent != null)
				closeDialogueUIEvent.RaiseEvent();

			ToggleCameras(false);

			yield return new WaitForSeconds(1f);

			if (endDialogue != null)
				endDialogue.RaiseEvent(m_CurrentStory);

			inputReader.advanceDialogueEvent -= OnAdvance;
			inputReader.EnableGameplayInput();

			_isInputEnabled = true;

			yield return null;
		}

		private void Increment()
		{
			m_Counter++;
		}

		private void ToggleCameras(bool enable)
		{
			gameCam.SetActive(!enable);
			dialogueCam.SetActive(enable);
		}

		/*
		private void InitEvents()
		{
			// Search dialogue events for dialogue id
			var links = TableDatabase.Get.FindLinks("dialogueOptionEvents", "dialogueId", m_CurrentStory.DialogueLines[m_Counter].ID);

			// Create all events
			int i = 0;
			foreach (var link in links)
			{
				bool nameFound = false;
				bool valueFound = false;
				bool svalueFound = false;

				string s = "";
				double d = 0;
				double d2 = 0;

				foreach (var field in link.Item2.Fields)
				{
					if (field.Key.ColumnName == "name")
					{
						nameFound = true;

						s = (string) field.Value.Data;
					}
					else if (field.Key.ColumnName == "value")
					{
						valueFound = true;
						d = (double) field.Value.Data;
					}
					else if (field.Key.ColumnName == "secondaryValue")
					{
						svalueFound = true;
						d2 = (double) field.Value.Data;
					}

					if (nameFound && valueFound && svalueFound)
					{
						break;
					}
				}

				string eventName = s;
				i++;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="onEnter"></param>
		private void CallEvents(bool onEnter) { }
		*/
	}
}
