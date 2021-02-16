using UnityEngine;

namespace DatabaseSync.Components
{
	// this script needs to be put on the actor, and takes care of the current step to accomplish.
	// the step contains a dialogue and maybe an event.

	public class RevisionController : MonoBehaviour
	{
		public UnityEngine.InputSystem.PlayerInput playerInput;
		public ActorSO Actor => actor;

		[Header("Data")]
		[SerializeField] private ActorSO actor;

		[SerializeField]
		private StorySO defaultDialogue;

		[Header("Listening to channels")]
		[SerializeField] private TaskChannelSO startTaskEvent;

		[SerializeField] private DialogueStoryChannelSO endDialogueEvent;
		[SerializeField] private DialogueActorChannelSO interactionEvent;

		[SerializeField] private VoidEventChannelSO winDialogueEvent;
		[SerializeField] private VoidEventChannelSO loseDialogueEvent;
		[SerializeField] private VoidEventChannelSO continueWithTask;
		[SerializeField] private VoidEventChannelSO endTaskEvent;
		[SerializeField] private VoidEventChannelSO closeDialogueUIEvent;

		[Header("Broadcasting on channels")]
		[SerializeField] private VoidEventChannelSO checkTaskValidityEvent;

		[SerializeField] private DialogueStoryChannelSO startDialogueEvent;

		[SerializeField] private TransformEventChannelSO startTransformDialogue;

		// check if character is active. An active character is the character concerned by the task.
		private bool _hasActiveTask;
		private TaskSO _currentTask;
		private StorySO _currentDialogue;

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

			if (winDialogueEvent != null)
			{
				winDialogueEvent.OnEventRaised += PlayWinDialogue;
			}

			if (loseDialogueEvent != null)
			{
				loseDialogueEvent.OnEventRaised += PlayLoseDialogue;
			}

			if (endTaskEvent != null)
			{
				endTaskEvent.OnEventRaised += EndTask;
			}

			if (continueWithTask != null)
			{
				continueWithTask.OnEventRaised += ContinueWithTask;
			}

			if(closeDialogueUIEvent != null)
			{
				closeDialogueUIEvent.OnEventRaised += () => playerInput.SwitchCurrentActionMap("Gameplay");
			}
		}

		//play default dialogue if no step
		void PlayDefaultDialogue()
		{
			if (defaultDialogue != null)
			{
				_currentDialogue = defaultDialogue;
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
			_hasActiveTask = true;
		}

		// start a dialogue when interaction
		// some Tasks need to be instantaneous. And do not need the interact button.
		// when interaction again, restart same dialogue.
		void InteractWithCharacter(ActorSO actorToInteractWith)
		{
			if (actorToInteractWith == actor)
			{
				if (_hasActiveTask)
				{
					StartTask();
				}
				else
				{
					PlayDefaultDialogue();
				}
			}
		}

		public void InteractWithCharacter()
		{
			if (_hasActiveTask)
			{
				StartTask();
			}
			else
			{
				PlayDefaultDialogue();
			}
		}

		void StartTask()
		{
			if (_currentTask != null)
				if (_currentTask.DialogueBeforeTask != null)
				{
					_currentDialogue = _currentTask.DialogueBeforeTask;
					StartDialogue();
				}
				else
				{
					Debug.LogError("Task without dialogue registering not implemented.");
				}
		}

		void StartDialogue()
		{
			if (startTransformDialogue != null)
			{
				startTransformDialogue.RaiseEvent(transform);
			}

			if (startDialogueEvent != null)
			{
				startDialogueEvent.RaiseEvent(_currentDialogue);
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
		void EndDialogue(StorySO dialogue)
		{
			//depending on the dialogue that ended, do something. The dialogue type can be different from the current dialogue type
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
				StartTask();
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
			_currentDialogue = defaultDialogue;
		}
	}
}
