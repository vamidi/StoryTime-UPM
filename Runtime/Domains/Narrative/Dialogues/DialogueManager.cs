using System;
using System.Collections;

using UnityEngine;
using Cinemachine;
using UnityEngine.TestTools;

namespace StoryTime.Domains.Narrative.Dialogues
{
	using UI.Utilities;
	using Game.Input.ScriptableObjects;
	using StoryTime.Domains.Events.ScriptableObjects;
	using StoryTime.Domains.Narrative.Dialogues.Events.UI;
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
	using StoryTime.Domains.VisualScripting.Data.ScriptableObjects;
	using StoryTime.Domains.Narrative.Dialogues.ScriptableObjects.Events;
	using StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Events;
	using StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues;


	/// <summary>
	/// The Dialogue manager keeps track of the dialogue in the game.
	/// </summary>
	public class DialogueManager: MonoBehaviour, 
#if UNITY_INCLUDE_TESTS
		IMonoBehaviourTest
#endif
	{
		public GameObject continueBtn;

		private bool ReachedEndOfDialogue
		{
			get
			{
				return _currentDialogue switch
				{
					DialogueNode dialogueNode => dialogueNode.Children.Count == 0,
					StartNode startNode => startNode.Child == null,
					_ => true
				};
			}
		}

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

		[SerializeField] private DialogueStoryChannelSO endStoryEvent;

		[SerializeField] private VoidEventChannelSO continueWithTask;
		[SerializeField] private VoidEventChannelSO closeDialogueUIEvent;
		[SerializeField] private VoidEventChannelSO closeChoiceUIEvent;

		[Tooltip("This will trigger an event when a dialogue or an option appears")]
		[SerializeField] private DialogueEventChannelSO dialogueEvent;
		
		[SerializeField] private TMPVertexAnimator textAnimator;

		private StorySO _currentStory;
		// private CharacterSO _currentActor;
		private Node _currentDialogue;

		private bool _isInputEnabled;
		private bool _canContinue;

		void Start()
		{
			return;
			
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

			if(textAnimator)
				textAnimator.allCharactersRevealed.AddListener(() =>
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
			// _currentActor = storyDataSo.Character;

			DisplayDialogueLine(storyDataSo.Dialogue);
			ToggleCameras(true);
		}

		/// <summary>
		/// Override with new created event nodes
		/// </summary>
		/// <param name="node"></param>
		protected void ProcessEventNodes(EventNode node)
		{
			if (dialogueEvent == null && node && node.EventName != String.Empty)
			{
				return;
			}

			// Call event when the dialogue begins

			if (node is BoolEventNode boolEventNode)
			{
				dialogueEvent.RaiseEvent(boolEventNode.EventName, boolEventNode.Value);
			}

			if (node is IntEventNode intEventNode)
			{
				dialogueEvent.RaiseEvent(intEventNode.EventName, intEventNode.Value);
			}

			if (node is StringEventNode stringEventNode)
			{
				dialogueEvent.RaiseEvent(stringEventNode.EventName, stringEventNode.Value);
			}
		}

		/// <summary>
		/// Displays DialogueData in the UI, one by one.
		/// Start interaction with the NPC
		/// </summary>
		/// <param name="storyDataSo"></param>
		private void Interact(StorySO storyDataSo)
		{
			// _currentActor = storyDataSo.Character;

			BeginDialogueStory(storyDataSo);
			// TODO fix me
			// DisplayDialogue(storyDataSo.rootNode);
			ToggleCameras(true);
		}

		private void DisplayDialogue(Node node)
		{
			_canContinue = false;

			if(node is IDialogueNode dialogueNode)
			{
				// send event out before the dialogue starts
				// InitEvents();
				// CallEvents(true);

				DisplayDialogueLine(dialogueNode.DialogueLine);
				SetActiveDialogue(node);
			}

			ProcessEventNodes(node as EventNode);
		}

		/// <summary>
		/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
		/// This function is also called by <see cref="DialogueLine"/> from clips on Timeline during cutscenes.
		/// </summary>
		/// <param name="dialogueLine"></param>
		private void DisplayDialogueLine(DialogueLine dialogueLine)
		{
			_canContinue = false;

			if (openUIDialogueEvent != null)
			{
				// send event out before the dialogue starts
				// InitEvents();
				// CallEvents(true);

				openUIDialogueEvent.RaiseEvent(dialogueLine);
			}
			ToggleContinueBtn(false);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="nextDialogue"></param>
		private void ShowNextDialogue(Node nextDialogue)
		{
			// TODO make this work with increment only instead of setting the next dialogue.
			// increment to the next dialogue sequence
			DialogueChoiceEndAndCloseUI(true);
			DisplayDialogue(nextDialogue);
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
		private void BeginDialogueStory(StorySO storyDataSo)
		{
			inputReader.EnableDialogueInput();
			_currentStory = storyDataSo;
			_isInputEnabled = false;
			StopAllCoroutines();
		}

		private void SetActiveDialogue(Node dialogue)
		{
			_currentDialogue = dialogue;
		}

		/// <summary>
		/// Show the full dialogue
		/// </summary>
		private void Skip()
		{
			textAnimator.ShowAllCharacters();
		}

		/// <summary>
		/// Show the next dialogue
		/// This is called when the player clicks on the continue button.
		/// </summary>
		private void OnAdvance()
		{
			#region NEW_ANIMATOR

			if (!textAnimator.HasRevealedAllCharacters)
			{
				Skip();
				return;
			}

			#endregion
			
			
			if (textAnimator.IsRevealing && !textAnimator.IsAllRevealed())
			{
				Skip();
				return;
			}

			if (_currentDialogue is DialogueNode dialogueNode && dialogueNode.Choices.Count > 0)
			{
				DisplayChoices(dialogueNode);
				return;
			}

			// Hide the option when advancing. // TODO this will be helpful for later when we use index instead of next dialogue
			DialogueChoiceEndAndCloseUI();

			if (!ReachedEndOfDialogue && _canContinue)
			{
				_currentDialogue = _currentDialogue switch
				{
					DialogueNode => null,
					StartNode startNode => startNode.Child,
					_ => null
				};

				// TODO grab the actor from the node editor.
				DisplayDialogue(_currentDialogue);
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

		private void DisplayChoices(DialogueNode dialogueNode)
		{
			inputReader.advanceDialogueEvent -= OnAdvance;
			if (makeDialogueChoiceEvent != null)
			{
				makeDialogueChoiceEvent.OnChoiceEventRaised += MakeDialogueChoice;
			}

			if (showChoicesUIEvent != null)
			{
				showChoicesUIEvent.RaiseEvent(dialogueNode.Choices);
			}
		}

		private void MakeDialogueChoice(DialogueChoice choice)
		{
			if (makeDialogueChoiceEvent != null)
			{
				makeDialogueChoiceEvent.OnChoiceEventRaised -= MakeDialogueChoice;
			}

			if (dialogueEvent != null && choice.DialogueChoiceEvent != String.Empty)
			{
				dialogueEvent.RaiseEvent(choice.DialogueChoiceEvent, _currentStory);
			}
			else if (choice.ActionType == ChoiceActionType.ContinueWithTask)
			{
				if (continueWithTask != null)
					continueWithTask.RaiseEvent();
			}

			var nextNode = FindSelectedChoiceNode(choice);
			if (nextNode != null)
			{
				ShowNextDialogue(nextNode);
			}
			else
				StartCoroutine(DialogueEndedAndCloseDialogueUI());
		}

		private Node FindSelectedChoiceNode(DialogueChoice choice)
		{
			if (_currentDialogue is DialogueNode dialogueNode)
			{
				var idx = dialogueNode.Choices.IndexOf(choice);
				if (idx != -1)
				{
					return dialogueNode.Children[idx + 1];
				}
			}

			return null;
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

			if (endStoryEvent != null) endStoryEvent.RaiseEvent(_currentStory);

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
#if UNITY_INCLUDE_TESTS
		private int frameCount;
		public bool IsTestFinished
		{
			get { return frameCount > 10; }
		}

		void Update()
		{
			frameCount++;
		}
#endif
	}
}
