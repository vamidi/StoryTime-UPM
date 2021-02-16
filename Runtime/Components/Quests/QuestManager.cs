using System.Collections.Generic;
using UnityEngine;

namespace DatabaseSync.Components.Quest
{
	public class QuestManager : MonoBehaviour
	{
		[Header("Data")]
		[SerializeField] private List<QuestSO> quests;

		// TODO Add inventory
		// [SerializeField] private Inventory _inventory = default;

		[Header("Listening to channels")]
		[SerializeField] private VoidEventChannelSO checkTaskValidityEvent;

		[SerializeField] private DialogueStoryChannelSO endDialogueEvent;

		[SerializeField] private DialogueChoiceEventChannelSO acceptQuestEvent;

		[Header("Broadcasting on channels")]
		[SerializeField] private TaskChannelSO startTaskEvent;

		[SerializeField] private VoidEventChannelSO endTaskEvent;

		[Tooltip("Dialogue to show when the condition is met")]
		[SerializeField] private VoidEventChannelSO winDialogueEvent;

		[Tooltip("Dialogue condition to show when the condition is not met")]
		[SerializeField] private VoidEventChannelSO loseDialogueEvent;

		[Tooltip("Item we need to give for the quest")]
		[SerializeField] private ItemEventChannelSO giveItemEvent;

		[Tooltip("Reward we get for completing the quest")]
		[SerializeField] private ItemEventChannelSO rewardItemEvent;

		private QuestSO _currentQuest;
		private TaskSO _currentTask;
		private int m_CurrentQuestIndex;
		private int m_CurrentTaskIndex;

		void Start()
		{
			if (checkTaskValidityEvent != null)
			{
				checkTaskValidityEvent.OnEventRaised += CheckTaskValidity;
			}

			if (endDialogueEvent != null)
			{
				endDialogueEvent.OnEventRaised += EndDialogue;
			}

			if (acceptQuestEvent != null)
			{
				acceptQuestEvent.OnEventRaised += OnQuestAccepted;
			}

			StartGame();
		}

		void OnQuestAccepted(string eventName, Object quest)
		{
			if(eventName == "AcceptQuest")
				quests.Add(quest as QuestSO);
		}

		void StartGame()
		{
			// Add code for saved information
			m_CurrentQuestIndex = 0;
			if (quests != null)
			{
				m_CurrentQuestIndex = quests.FindIndex(o => o.IsDone == false);
				if (m_CurrentQuestIndex >= 0)
					StartQuest();
			}
		}

		void StartQuest()
		{
			if (quests != null)
			{
				if (quests.Count > m_CurrentQuestIndex)
				{
					_currentQuest = quests[m_CurrentQuestIndex];
					m_CurrentTaskIndex = 0;
					m_CurrentTaskIndex = _currentQuest.Tasks.FindIndex(o => o.IsDone == false);
					if (m_CurrentTaskIndex >= 0)
						StartTask();
				}
			}
		}

		void StartTask()
		{
			if (_currentQuest.Tasks != null)
			{
				if (_currentQuest.Tasks.Count > m_CurrentTaskIndex)
				{
					_currentTask = _currentQuest.Tasks[m_CurrentTaskIndex];
					startTaskEvent.RaiseEvent(_currentTask);
				}
			}
		}

		void CheckTaskValidity()
		{
			if (_currentQuest != null)
				if (_currentQuest.Tasks != null)
					if (_currentQuest.Tasks.Count > m_CurrentTaskIndex)
					{
						_currentTask = _currentQuest.Tasks[m_CurrentTaskIndex];
						switch (_currentTask.Type)
						{
							case TaskCompletionType.Collect:
								/*
								if (_inventory.Contains(_currentStep.Item))
								{
									_inventory.Contains(_currentStep.Item);
									_giveItemEvent.RaiseEvent(_currentStep.Item);
									//Trigger win dialogue
									if (_winDialogueEvent != null)
									{
										_winDialogueEvent.OnEventRaised();
									}
								}
								else
								{
									//trigger lose dialogue
									if (_loseDialogueEvent != null)
									{
										_loseDialogueEvent.OnEventRaised();
									}
								}
								*/
								break;
							case TaskCompletionType.Defeat:
								break;
							case TaskCompletionType.Interact:
								break;
							case TaskCompletionType.Defend:
								break;
							case TaskCompletionType.Talk:
								// dialogue has already been played
								EndTask();
								break;
						}
					}


		}

		void EndDialogue(StorySO dialogue)
		{
			// depending on the dialogue that ended, do something
			switch (dialogue.DialogueType)
			{
				case DialogueType.WinDialogue:
					EndTask();
					break;
			}
		}

		void EndTask()
		{
			_currentTask = null;

			if (quests != null)
			{
				if (quests.Count > m_CurrentQuestIndex)
				{
					if (quests[m_CurrentQuestIndex].Tasks != null)
					{
						if (quests[m_CurrentQuestIndex].Tasks.Count > m_CurrentTaskIndex)
						{
							if (endTaskEvent != null)
								endTaskEvent.RaiseEvent();

							TaskSO task = quests[m_CurrentQuestIndex].Tasks[m_CurrentTaskIndex];

							// finish the task
							task.FinishTask();

							// reward
							rewardItemEvent.RaiseEvent(task.Item);

							if (quests[m_CurrentQuestIndex].Tasks.Count > m_CurrentTaskIndex + 1)
							{
								m_CurrentTaskIndex++;
								StartTask();
							}
							else
							{
								EndQuest();
							}
						}
					}
				}
			}

			void EndQuest()
			{
				if (quests != null)
				{
					if (quests.Count > m_CurrentQuestIndex)
					{
						quests[m_CurrentQuestIndex].FinishQuest();

						if (quests.Count < m_CurrentQuestIndex + 1)
						{
							m_CurrentQuestIndex++;
							StartQuest();
						}
					}
				}
			}
		}
	}
}
