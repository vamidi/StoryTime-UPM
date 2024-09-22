using System;
using System.Collections.Generic;
using StoryTime.Domains.Narrative.Stories;
using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Narrative.Tasks.ScriptableObjects
{
	using Events;
	using ItemManagement.Inventory;
	using StoryTime.Domains.Database.ScriptableObjects;
	using StoryTime.Domains.Game.Characters.ScriptableObjects;
	using StoryTime.Domains.Game.NPC.Enemies.ScriptableObjects;
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;

	public enum TaskCompletionType
	{
		None, //
		Collect, // collect an or multiple item(s) 1
		Defeat, // defeat a certain enemy. 2
		Talk, // Talk to a npc. 3
		Interact, // Interact with an object 4
		Defend // Defend an object or npc. 5
	}

	[CreateAssetMenu(fileName = "Task", menuName = "StoryTime/Game/Narrative/Task", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class TaskSO : TableBehaviour, IReadOnlyTask
	{
		public String NextId { get => _nextId; set => _nextId = value; }
		public string Title => title;
		public string Description => description;
		public int Distance => distance;
		public bool Hidden { get => hidden; set => hidden = value; }
		public String Npc { get => npc; set => npc = value; }
		public EnemySO.EnemyCategory EnemyCategory => enemyCategory;
		public IReadOnlyStory Parent => parent;
		public uint RequiredCount { get => requiredCount; set => requiredCount = value; }
		public List<ItemStack> Items { get => items; set => items = value; }
		public TaskCompletionType Type { get => type; set => type = value; }
		public IReadOnlyStory StoryBeforeTask => storyBeforeTask;
		public IReadOnlyStory WinStory => completeStory;
		public IReadOnlyStory LoseStory => incompleteStory;
		public bool IsDone => isDone;
		public CharacterSO Character => character;
		public TaskEventChannelSO StartTaskEvent => endTaskEvent;
		public TaskEventChannelSO EndTaskEvent => endTaskEvent;

		[SerializeField, Tooltip("Title of the task")]
		private string title;

		[SerializeField, Tooltip("The description of the mission")]
		private string description;

		[SerializeField, Tooltip("Distance to the task in meters")]
		private int distance;

		[Tooltip("Whether the mission is hidden")]
		[SerializeField] private bool hidden;

		// the next task that the player needs to perform.
		private String _nextId = String.Empty;

		// Reference to the interactable id, could be a monster or a npc
		// It could be even a reference to a group of monsters of some type.
		private String npc = String.Empty;

		[SerializeField, Tooltip("Parent of the task.")] private IReadOnlyStory parent;

		// TODO show category instead of number
		[SerializeField, HideInInspector, Tooltip("Which enemy category do we need to hunt")] private EnemySO.EnemyCategory enemyCategory = new ();

		// Reference to the parent, which is the quest.

		[Tooltip("Requirement amount to complete the mission")]
		[SerializeField] private uint requiredCount;

		// Keep reference to the amount we killed or collected.
		private int m_Count;

		[Tooltip("The Character this mission belongs or we will need to interact with")]
		[SerializeField] private CharacterSO character;

		[Tooltip("The story that will be displayed before an action, if any")]
		[SerializeField] private IReadOnlyStory storyBeforeTask;

		[Tooltip("The story that will be displayed when the step is achieved")]
		[SerializeField] private IReadOnlyStory completeStory;

		[Tooltip("The story that will be displayed if the step is not achieved yet")]
		[SerializeField] private IReadOnlyStory incompleteStory;

		[Tooltip("The item to check/give/reward (can be multiple)")]
		[SerializeField] private List<ItemStack> items;

		[Tooltip("The type of the task")]
		[SerializeField] private TaskCompletionType type;

		[SerializeField, Tooltip("An event that we trigger when a certain task is started.")]
		private TaskEventChannelSO startTaskEvent;

		[SerializeField, Tooltip("An event that we trigger when a certain task is finished.")]
		private TaskEventChannelSO endTaskEvent;

		// [SerializeField, Tooltip("Task event value we want to attach.")] private TaskEventSO taskEvent;

		[SerializeField] bool isDone;

		public virtual void OnEnable()
		{
			m_Count = 0;
			isDone = false;
		}

		// Increase the amount of
		public void Increment()
		{
			m_Count++;
		}

		/// <summary>
		/// This function will validate whether the required amount is met.
		/// </summary>
		/// <returns></returns>
		public bool Validate()
		{
			return m_Count >= requiredCount;
		}

		public void StartTask()
		{
			// startTaskEvent?.RaiseEvent(this, taskEvent);
		}

		public void FinishTask()
		{
			isDone = true;
			// if (endTaskEvent != null) endTaskEvent.RaiseEvent(this, taskEvent);
		}

		public TaskSO(): base("tasks", "description") {}
	}
}
