using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Cinemachine;

namespace StoryTime.Components
{
	using ScriptableObjects;
	using Input.ScriptableObjects;
	using Events.ScriptableObjects;

	/// <summary>
	/// The Dialogue manager keeps track of the dialogue in the game.
	/// </summary>
	public class DialogueManager: MonoBehaviour
	{
		public GameObject continueBtn;

		private bool ReachedEndOfDialogue => m_CurrentDialogue.NextDialogue == null;

		[SerializeField] private BaseInputReader inputReader;

		[Header("Cameras")]
		public GameObject gameCam;
		public GameObject dialogueCam;

		[Header("Targets")]
		public CinemachineTargetGroup targetGroup;

		[Header("Listening on channels")]
		[SerializeField] private DialogueStoryChannelSO startStoryEvent;
		[SerializeField] private DialogueChannelSO startDialogue;
		[SerializeField] private DialogueChoiceChannelSO makeDialogueChoiceEvent;

		[SerializeField] private TransformEventChannelSO startTransformDialogue;

		[Header("BroadCasting on channels")]
		[SerializeField] private DialogueLineChannelSO openUIDialogueEvent;
		[SerializeField] private DialogueChoiceChannelSO showChoicesUIEvent;

		// [SerializeField] private DialogueStoryChannelSO endStoryEvent;

		[SerializeField] private VoidEventChannelSO continueWithTask;
		[SerializeField] private VoidEventChannelSO closeDialogueUIEvent;
		[SerializeField] private VoidEventChannelSO closeChoiceUIEvent;

		[Tooltip("This will trigger an event when a dialogue or an option appears")]
		[SerializeField] private DialogueEventChannelSO dialogueEvent;

		[SerializeField] private TMPVertexAnimator revealer;

		private SimpleStorySO m_CurrentStory;
		private CharacterSO m_CurrentActor;
		private IDialogueLine m_CurrentDialogue;

		private bool _isInputEnabled;
		private bool _canContinue;

		void Start()
		{
			if (startStoryEvent != null)
			{
				startStoryEvent.OnEventRaised += Interact;
			}

			if (startDialogue != null)
				startDialogue.OnEventRaised += DisplayDialogueLine;

			if (startTransformDialogue != null) // Set the target to the person we are talking to.
				startTransformDialogue.OnEventRaised += (tr) => targetGroup.m_Targets[1].target = tr;

			if(gameCam == null)
				gameCam = GameObject.FindWithTag("GameCamera");

			if(dialogueCam == null)
				dialogueCam = GameObject.FindWithTag("DialogueCamera");

			if (dialogueCam)
			{
				var virtualCam = dialogueCam.GetComponent<CinemachineVirtualCamera>();
				virtualCam.Follow = virtualCam.LookAt = targetGroup.transform;
			}

			ToggleCameras(false);

			if(revealer)
				revealer.allRevealed.AddListener(() =>
				{
					_canContinue = true;
					ToggleContinueBtn(true);
				});

			var player = GameObject.FindWithTag("Player");
			if(player)
			{
				targetGroup.m_Targets[0].target = player.transform;
			}

			if(inputReader)
				inputReader.advanceDialogueEvent += OnAdvance;
		}

		/// <summary>
		/// Displays DialogueData in the UI, one by one.
		/// Start interaction with the NPC
		/// </summary>
		/// <param name="storyDataSo"></param>
		public void Interact(SimpleStorySO storyDataSo)
		{
			BeginDialogueStory(storyDataSo);
			DisplayDialogueLine(storyDataSo.StartDialogue, storyDataSo.Character);
			ToggleCameras(true);
		}

		/// <summary>
		/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
		/// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
		/// </summary>
		/// <param name="dialogueLine"></param>
		/// <param name="character"></param>
		public void DisplayDialogueLine(IDialogueLine dialogueLine, CharacterSO character)
		{
			_canContinue = false;

			if (openUIDialogueEvent != null)
			{
				// send event out before the dialogue starts
				// InitEvents();
				// CallEvents(true);

				openUIDialogueEvent.RaiseEvent(dialogueLine, character);
			}
			ToggleContinueBtn(false);

			// Call event when the dialogue begins
			if (dialogueEvent != null && dialogueLine.DialogueEvent.EventName != String.Empty)
			{
				dialogueEvent.RaiseEvent(dialogueLine.DialogueEvent.EventName, dialogueLine.DialogueEvent.Value);
			}

			m_CurrentActor = character;

			SetActiveDialogue(dialogueLine);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="nextDialogueLineSo"></param>
		public void ShowNextDialogue(IDialogueLine nextDialogueLineSo)
		{
			// TODO make this work with increment only instead of setting the next dialogue.
			// increment to the next dialogue sequence
			DialogueChoiceEndAndCloseUI(true);
			DisplayDialogueLine(nextDialogueLineSo, m_CurrentActor);
		}

		public void ToggleCameras(bool enable)
		{
			if(gameCam)
				gameCam.SetActive(!enable);

			if(dialogueCam)
				dialogueCam.SetActive(enable);
		}

		/// <summary>
		/// Prepare DialogueManager when first time displaying DialogueData.
		/// <param name="storyDataSo"></param>
		/// </summary>
		private void BeginDialogueStory(SimpleStorySO storyDataSo)
		{
			inputReader.EnableDialogueInput();
			m_CurrentStory = storyDataSo;
			_isInputEnabled = false;
			StopAllCoroutines();
		}

		private void SetActiveDialogue(IDialogueLine dialogue)
		{
			m_CurrentDialogue = dialogue;
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
			bool hasOptions = m_CurrentDialogue.Choices.Count > 0;
			if (revealer.IsRevealing && !revealer.IsAllRevealed())
			{
				Skip();
				return;
			}

			if (hasOptions)
			{
				DisplayChoices(m_CurrentDialogue.Choices);
				return;
			}

			// Hide the option when advancing. // TODO this will be helpful for later when we use index instead of next dialogue
			DialogueChoiceEndAndCloseUI();

			if (!ReachedEndOfDialogue && _canContinue)
			{
				m_CurrentDialogue = m_CurrentDialogue.NextDialogue;
				// TODO grab the actor from the node editor.

				DisplayDialogueLine(m_CurrentDialogue, m_CurrentActor);
				return;
			}

			// if we reached the end of the dialogue then close everything.
			StartCoroutine(DialogueEndedAndCloseDialogueUI());
		}

		private void ToggleContinueBtn(bool toggle)
		{
			if(continueBtn)
				continueBtn.gameObject.SetActive(toggle);
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

			if (dialogueEvent != null && choice.DialogueChoiceEvent != String.Empty)
			{
				dialogueEvent.RaiseEvent(choice.DialogueChoiceEvent, m_CurrentStory);
			}
			else if (choice.ActionType == ChoiceActionType.ContinueWithTask)
			{
				if (continueWithTask != null)
					continueWithTask.RaiseEvent();
			}

			if (choice.NextDialogue != null)
			{
				ShowNextDialogue(choice.NextDialogue);
				SetActiveDialogue(choice.NextDialogue);
			}
			else
				StartCoroutine(DialogueEndedAndCloseDialogueUI());

		}

		private void DialogueChoiceEndAndCloseUI(bool resubscribe = false)
		{
			if (closeChoiceUIEvent != null)
				closeChoiceUIEvent.RaiseEvent();

			if (resubscribe)
			{
				inputReader.advanceDialogueEvent += OnAdvance;
			}
		}

		private IEnumerator DialogueEndedAndCloseDialogueUI()
		{
			if (_isInputEnabled) yield break;

			yield return new WaitForSeconds(1.0f);

			if (closeDialogueUIEvent != null)
				closeDialogueUIEvent.RaiseEvent();

			ToggleCameras(false);

			yield return new WaitForSeconds(1.5f);

			// if (endStoryEvent != null) endStoryEvent.RaiseEvent(m_CurrentStory, null);

			DialogueChoiceEndAndCloseUI();
			inputReader.EnableGameplayInput();

			_isInputEnabled = true;

			yield return null;
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
