using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;
using UnityEditor.Localization;


using StoryTime.Database.ScriptableObjects;
namespace StoryTime.Components.ScriptableObjects
{
	using Events.ScriptableObjects;

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
	public partial class TaskSO : LocalizationBehaviour
	{
		public uint NextId { get => nextId; set => nextId = value; }
		public LocalizedString Description => description;
		public bool Hidden { get => hidden; set => hidden = value; }
		public uint Npc { get => npc; set => npc = value; }
		public uint EnemyCategory => enemyCategory;
		public uint ParentId { get; set; } = UInt32.MaxValue;
		public uint RequiredCount { get => requiredCount; set => requiredCount = value; }
		public List<ItemStack> Items { get => items; set => items = value; }
		public TaskCompletionType Type { get => type; set => type = value; }
		public StorySO StoryBeforeTask => storyBeforeTask;
		public StorySO WinStory => completeStory;
		public StorySO LoseStory => incompleteStory;
		public bool IsDone => isDone;
		public CharacterSO Character => character;
		public TaskEventChannelSO StartTaskEvent => endTaskEvent;
		public TaskEventChannelSO EndTaskEvent => endTaskEvent;

		[SerializeField, HideInInspector, Tooltip("The description of the mission")]
		private LocalizedString description;

		[Tooltip("Whether the mission is hidden")]
		[SerializeField] private bool hidden;

		// the next task that the player needs to perform.
		private uint nextId = UInt32.MaxValue;

		// Reference to the interactable id, could be a monster or a npc
		// It could be even a reference to a group of monsters of some type.
		private uint npc = UInt32.MaxValue;

		// TODO show category instead of number
		[SerializeField, HideInInspector, Tooltip("Which enemy category do we need to hunt")] private uint enemyCategory = UInt32.MaxValue;

		// Reference to the parent, which is the quest.

		[Tooltip("Requirement amount to complete the mission")]
		[SerializeField] private uint requiredCount;

		// Keep reference to the amount we killed or collected.
		private int m_Count;

		[Tooltip("The Character this mission belongs or we will need to interact with")]
		[SerializeField] private CharacterSO character;

		[Tooltip("The story that will be displayed before an action, if any")]
		[SerializeField] private StorySO storyBeforeTask;

		[Tooltip("The story that will be displayed when the step is achieved")]
		[SerializeField] private StorySO completeStory;

		[Tooltip("The story that will be displayed if the step is not achieved yet")]
		[SerializeField] private StorySO incompleteStory;

		[Tooltip("The item to check/give/reward (can be multiple)")]
		[SerializeField] private List<ItemStack> items;

		[Tooltip("The type of the task")]
		[SerializeField] private TaskCompletionType type;

		[SerializeField, Tooltip("An event that we trigger when a certain task is started.")]
		private TaskEventChannelSO startTaskEvent;

		[SerializeField, Tooltip("An event that we trigger when a certain task is finished.")]
		private TaskEventChannelSO endTaskEvent;

		[SerializeField, Tooltip("Task event value we want to attach.")] private TaskEventSO taskEvent;

		[SerializeField] bool isDone;

		protected override void OnTableIDChanged()
		{
			base.OnTableIDChanged();
			Initialize();
		}

		public virtual void OnEnable()
		{
#if UNITY_EDITOR
			Initialize();
#endif
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
			if(startTaskEvent) startTaskEvent.RaiseEvent(this, taskEvent);
		}

		public void FinishTask()
		{
			isDone = true;
			if (endTaskEvent != null) endTaskEvent.RaiseEvent(this, taskEvent);
		}

		TaskSO(): base("tasks", "description") {}

		private void Initialize()
		{
			if (ID != UInt32.MaxValue)
			{
				collection = overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Task Descriptions");
				// Only get the first dialogue.
				var entryId = (ID + 1).ToString();
				if(collection)
					description = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
				else
					Debug.LogWarning("Collection not found. Did you create any localization tables for Tasks");
			}
		}
	}
}
