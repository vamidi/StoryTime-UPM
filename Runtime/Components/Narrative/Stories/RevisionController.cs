using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace DatabaseSync.Components
{
	using Events;

	// this script needs to be put on the character, and takes care of the current task that the player has to accomplish.
	// the task contains a story and maybe an event.
    [AddComponentMenu("DatabaseSync/RevisionController")]
	public class RevisionController : MonoBehaviour
	{
		// TODO rename this to Character
		public CharacterSO Character => character;

		[Header("Data")]
		[SerializeField] private CharacterSO character;

		[SerializeField, Tooltip("Simple story we want to display")]
		private SimpleStorySO defaultStory;

		[SerializeField, Tooltip("Stories that belongs to this character")]
		private List<SimpleStorySO> stories;

		[Header("BroadCasting on channels")]
		[SerializeField] private VoidEventChannelSO onInteractionEnded;

		[Header("Listening to channels")]
		[SerializeField] private TaskEventChannelSO startTaskEvent;
		[SerializeField] private TaskEventChannelSO endTaskEvent;

		[SerializeField] private VoidEventChannelSO continueWithTask;
		[SerializeField] private VoidEventChannelSO completeDialogueEvent;
		[SerializeField] private VoidEventChannelSO incompleteDialogueEvent;
		[SerializeField] private VoidEventChannelSO closeDialogueUIEvent;

		// This is for each dialogue we call an even
		[SerializeField] private DialogueLineChannelSO endDialogueEvent;
		[SerializeField] private DialogueCharacterChannelSO interactionEvent;

		[Header("Broadcasting on channels")]
		[SerializeField] private VoidEventChannelSO checkTaskValidityEvent;

		[SerializeField] private DialogueStoryChannelSO startStoryEvent;
		// TODO enable the dialogue manager to show only dialogue.
		// [SerializeField] private DialogueLineChannelSO startDialogueEvent;
		[SerializeField] private TransformEventChannelSO startTransformDialogue;

		[Header("Revision"), Tooltip("Determines in which state the current controller is for the story.")]
		[SerializeField] private int revisionId = -1;

		// check if character is active. An active character is the character concerned by the task.
		private bool _hasActiveStory;
		private bool _hasActiveTask;
		private TaskSO _currentTask;
		private SimpleStorySO _currentStory;

		public void TurnToPlayer(Vector3 playerPos)
		{
			// transform.DOLookAt(playerPos, Vector3.Distance(transform.position, playerPos) / 5);
			// string turnMotion = isRightSide(transform.forward, playerPos, Vector3.up) ? "rturn" : "lturn";
			// animator.SetTrigger(turnMotion);
		}

		/// <summary>
		/// Update current revision.
		/// </summary>
		/// <param name="newId"></param>
		public void UpdateRevisionId(int newId = -1)
		{
			if (newId == -1)
				revisionId++;
			else revisionId = newId;
		}

		private void Start()
		{
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

			if (continueWithTask != null)
			{
				continueWithTask.OnEventRaised += ContinueWithTask;
			}

			if(closeDialogueUIEvent != null)
			{
				closeDialogueUIEvent.OnEventRaised += () =>
				{
					if (onInteractionEnded != null) onInteractionEnded.RaiseEvent();
				};
			}

			_hasActiveStory = defaultStory != null;
		}

		// play default dialogue if no step
		void PlayDefaultDialogue()
		{
			if (defaultStory != null)
			{
				_currentStory = defaultStory;
				StartDialogue();
			}
			else
			{
				// if we have nothing to show go back to normal input
				if (onInteractionEnded != null) onInteractionEnded.RaiseEvent();
			}
		}

		// play dialogue of the story
		void PlayStoryDialogue()
		{
			if (defaultStory != null)
			{
				_currentStory = defaultStory;
				StartDialogue();
			}
		}

		void CheckTaskInvolvement(TaskSO task, TaskEventSO value)
		{
			if (character == task.Character)
			{
				RegisterTask(task);
			}
		}

		// register a step
		void RegisterTask(TaskSO task)
		{
			_currentTask = task;
			// Find me a story where the task belongs to.
			_currentStory = stories.FirstOrDefault((s) => s.ID == task.ParentId) ?? defaultStory;
			revisionId = stories.FindIndex(s => s == _currentStory);
			Debug.Log(revisionId);
			_hasActiveTask = true;
		}

		// start a dialogue when interaction
		// some Tasks need to be instantaneous. And do not need the interact button.
		// when interaction again, restart same dialogue.
		void InteractWithCharacter(CharacterSO actorToInteractWith)
		{
			if (actorToInteractWith == character)
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
				else if (_currentTask.StoryBeforeTask != null)
				{
					_currentStory = _currentTask.StoryBeforeTask;
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
				startStoryEvent.RaiseEvent(_currentStory);
			}
		}

		void PlayLoseDialogue()
		{
			if (_currentTask != null)
				if (_currentTask.LoseStory != null)
				{
					_currentStory = _currentTask.LoseStory;
					StartDialogue();
				}

		}

		void PlayWinDialogue()
		{
			if (_currentTask != null)
				if (_currentTask.WinStory != null)
				{
					_currentStory = _currentTask.WinStory;
					StartDialogue();
				}
		}

		//End dialogue
		void EndDialogue(IDialogueLine dialogue, CharacterSO actorSo)
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
					if (_currentTask.StoryBeforeTask != null)
					{
						_currentStory = _currentTask.StoryBeforeTask;
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

		void EndTask(TaskSO stepToFinish, TaskEventSO value)
		{
			if (stepToFinish == _currentTask)
				UnregisterTask();
			else
			{
				PlayTaskDialogue();
			}
		}

		// void EndTask()
		// {
		// 	UnregisterTask();
		// }

		//unregister a step when it ends.
		private void UnregisterTask()
		{
			UpdateRevisionId();

			_currentTask = null;
			_hasActiveTask = false;
			_hasActiveStory = false;
			if (stories != null)
			{
				if (stories.Count > revisionId)
				{
					_currentStory = stories.FirstOrDefault(s => !s.IsDone);
				}
			}
		}

		// https://forum.unity.com/threads/left-right-test-function.31420/
		private bool isRightSide(Vector3 fwd, Vector3 targetDir, Vector3 up)
		{
			Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);        // right vector
			float dir = Vector3.Dot(right, targetDir.normalized);
			return dir > 0f;
		}
	}
}
