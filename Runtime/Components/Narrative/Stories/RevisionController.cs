using UnityEngine;

namespace DatabaseSync.Components
{
	using Events;

	// this script needs to be put on the actor, and takes care of the current step to accomplish.
	// the step contains a dialogue and maybe an event.

	public class RevisionController : MonoBehaviour
	{
		public UnityEngine.InputSystem.PlayerInput playerInput;
		// TODO rename this to Character
		public ActorSO Actor => actor;

		[Header("Data")]
		[SerializeField] private ActorSO actor;

		[SerializeField] private DialogueLine defaultDialogue;
		[SerializeField] private StorySO defaultStory;

		[Header("Listening to channels")]
		[SerializeField] private TaskEventChannelSO startTaskEvent;

		// This is for each dialogue we call an even
		[SerializeField] private DialogueLineChannelSO endDialogueEvent;
		[SerializeField] private DialogueActorChannelSO interactionEvent;

		[SerializeField] private VoidEventChannelSO completeDialogueEvent;
		[SerializeField] private VoidEventChannelSO incompleteDialogueEvent;
		[SerializeField] private VoidEventChannelSO continueWithTask;
		[SerializeField] private VoidEventChannelSO endTaskEvent;
		[SerializeField] private VoidEventChannelSO closeDialogueUIEvent;

		[SerializeField] private TaskEventChannelSO endStoryEvent;

		[Header("Broadcasting on channels")]
		[SerializeField] private VoidEventChannelSO checkTaskValidityEvent;

		[SerializeField] private DialogueStoryChannelSO startStoryEvent;
		// TODO enable the dialogue manager to show only dialogue.
		// [SerializeField] private DialogueLineChannelSO startDialogueEvent;
		[SerializeField] private TransformEventChannelSO startTransformDialogue;

		// check if character is active. An active character is the character concerned by the task.
		private bool _hasActiveStory;
		private bool _hasActiveTask;
		private TaskSO _currentTask;

		private IDialogueLine _currentDialogue;

		public void TurnToPlayer(Vector3 playerPos)
		{
			// transform.DOLookAt(playerPos, Vector3.Distance(transform.position, playerPos) / 5);
			// string turnMotion = isRightSide(transform.forward, playerPos, Vector3.up) ? "rturn" : "lturn";
			// animator.SetTrigger(turnMotion);
		}

		private void Start()
		{
			playerInput = FindObjectOfType<UnityEngine.InputSystem.PlayerInput>();
			if (endDialogueEvent != null)
			{
				endDialogueEvent.OnEventRaised += EndDialogue;
			}

			if (startTaskEvent != null)
			{
				startTaskEvent.OnEventRaised += CheckTaskInvolvement;
			}

			if (interactionEvent != null)
			{
				interactionEvent.OnEventRaised += InteractWithCharacter;
			}

			if (completeDialogueEvent != null)
			{
				completeDialogueEvent.OnEventRaised += PlayWinDialogue;
			}

			if (incompleteDialogueEvent != null)
			{
				incompleteDialogueEvent.OnEventRaised += PlayLoseDialogue;
			}

			if (endTaskEvent != null)
			{
				endTaskEvent.OnEventRaised += EndTask;
			}

			if (endStoryEvent != null)
			{
				endStoryEvent.OnEventRaised += EndTask;
			}

			if (continueWithTask != null)
			{
				continueWithTask.OnEventRaised += ContinueWithTask;
			}

			if(closeDialogueUIEvent != null)
			{
				closeDialogueUIEvent.OnEventRaised += () => playerInput.SwitchCurrentActionMap("Gameplay");
			}

			_hasActiveStory = defaultStory != null;
		}

		// play default dialogue if no step
		void PlayDefaultDialogue()
		{
			if (defaultDialogue != null)
			{
				_currentDialogue = defaultDialogue;
				StartDialogue();
			}
		}

		// play dialogue of the story
		void PlayStoryDialogue()
		{
			if (defaultStory != null)
			{
				_currentDialogue = defaultStory.StartDialogue;
				StartDialogue();
			}
		}

		void CheckTaskInvolvement(TaskSO task)
		{
			if (actor == task.Actor)
			{
				RegisterTask(task);
			}
		}

		// register a step
		void RegisterTask(TaskSO task)
		{
			_currentTask = task;
			_currentDialogue = defaultStory.StartDialogue;
			_hasActiveTask = true;
		}

		// start a dialogue when interaction
		// some Tasks need to be instantaneous. And do not need the interact button.
		// when interaction again, restart same dialogue.
		void InteractWithCharacter(ActorSO actorToInteractWith)
		{
			if (actorToInteractWith == actor)
			{
				if (_hasActiveStory)
				{
					PlayStoryDialogue();
				}
				else if (_hasActiveTask)
				{
					PlayTaskDialogue();
				}
				else
				{
					PlayDefaultDialogue();
				}
			}
		}

		public void InteractWithCharacter()
		{
			if (_hasActiveStory)
			{
				PlayStoryDialogue();
			}
			else if (_hasActiveTask)
			{
				PlayTaskDialogue();
			}
			else
			{
				PlayDefaultDialogue();
			}
		}

		/// <summary>
		/// St
		/// </summary>
		void PlayTaskDialogue()
		{
			if (_currentTask != null)
			{
				// if we have a task check the validation
				if (_hasActiveTask)
				{
					CheckTaskValidity();
				}
				// The player is going to get a tasks handed over
				else if (_currentTask.DialogueBeforeTask != null)
				{
					_currentDialogue = _currentTask.DialogueBeforeTask;
					StartDialogue();
				}
				else
				{
					Debug.LogError("Task without dialogue registering not implemented.");
				}
			}
		}

		void StartDialogue()
		{
			if (startTransformDialogue != null)
			{
				startTransformDialogue.RaiseEvent(transform);
			}

			if (startStoryEvent != null)
			{
				startStoryEvent.RaiseEvent(defaultStory, _currentDialogue);
			}
		}

		void PlayLoseDialogue()
		{
			if (_currentTask != null)
				if (_currentTask.LoseDialogue != null)
				{
					_currentDialogue = _currentTask.LoseDialogue;
					StartDialogue();
				}

		}

		void PlayWinDialogue()
		{
			if (_currentTask != null)
				if (_currentTask.WinDialogue != null)
				{
					_currentDialogue = _currentTask.WinDialogue;
					StartDialogue();
				}
		}

		//End dialogue
		void EndDialogue(IDialogueLine dialogue, ActorSO actorSo)
		{
			// depending on the dialogue that ended, do something. The dialogue type can be different from the current dialogue type
			switch (dialogue.DialogueType)
			{
				case DialogueType.StartDialogue:
					// Check the validity of the step
					CheckTaskValidity();
					break;
				case DialogueType.WinDialogue:
					// After playing the win dialogue close Dialogue and end step

					break;
				case DialogueType.LoseDialogue:
					// closeDialogue
					// replay start Dialogue if the lose Dialogue ended
					if (_currentTask.DialogueBeforeTask != null)
					{
						_currentDialogue = _currentTask.DialogueBeforeTask;
					}
					break;
				case DialogueType.DefaultDialogue:
					// close Dialogue
					// nothing happens if it's the default dialogue
					break;
			}
		}

		void ContinueWithTask()
		{
			CheckTaskValidity();
		}

		void CheckTaskValidity()
		{
			if (checkTaskValidityEvent != null)
			{
				checkTaskValidityEvent.RaiseEvent();
			}
		}

		void EndTask(TaskSO stepToFinish)
		{
			if (stepToFinish == _currentTask)
				UnregisterTask();
			else
			{
				PlayTaskDialogue();
			}
		}

		void EndTask()
		{
			UnregisterTask();
		}

		//unregister a step when it ends.
		void UnregisterTask()
		{
			_currentTask = null;
			_hasActiveTask = false;
			_hasActiveStory = false;
			_currentDialogue = defaultDialogue;
		}

		// https://forum.unity.com/threads/left-right-test-function.31420/
		bool isRightSide(Vector3 fwd, Vector3 targetDir, Vector3 up)
		{
			Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);        // right vector
			float dir = Vector3.Dot(right, targetDir.normalized);
			return dir > 0f;
		}
	}
}
