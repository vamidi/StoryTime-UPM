using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace DatabaseSync.Components
{
	using Binary;
	using Database;
	using Attributes;

	public enum QuestType
	{
		All,
		WorldQuests,
		SideQuests,
		// Extend with own quest types.
	}

	public enum DialogueType
	{
		StartDialogue,
		WinDialogue,
		LoseDialogue,
		DefaultDialogue
	}

	public enum ChoiceActionType
	{
		DoNothing,
		ContinueWithQuest,
		ContinueWithTask,
	}

	/// <summary>
	/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
	/// In future versions it might contain support for branching conversations.
	/// </summary>
	[CreateAssetMenu(fileName = "newStory", menuName = "DatabaseSync/Stories/Story", order = 51)]
	// ReSharper disable once InconsistentNaming
	public partial class StorySO : LocalizationBehaviour
	{
		public LocalizedString Title => title;
		public string CharacterName => characterName;
		public LocalizedString Description => description;
		public uint ParentId => parentId;
		public uint ChildId => childId;
		public QuestType TypeId => typeId;
		public List<TaskSO> Tasks => tasks;
		public bool IsDone => m_IsDone;
		public CharacterSO Character => character;

		// rename Dialogue line to story lines
		public DialogueLine StartDialogue => startDialogue;

		[Tooltip("The collection of tasks composing the Quest")]
		[SerializeField] private List<TaskSO> tasks = new List<TaskSO>();

		private bool m_IsDone;

		// public StorySO() : base("stories", "title") { }

		[SerializeField] private CharacterSO character;

		// Is being calculated in the story editor.
		[SerializeField] private DialogueLine startDialogue;

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		[SerializeField, Tooltip("Override where we should get the dialogue options data from.")]
		private bool overrideDialogueOptionsTable;

		[SerializeField, ConditionalField("overrideDialogueOptionsTable"), Tooltip("Table collection we are going to use for the sentence")]
		private StringTableCollection dialogueOptionsCollection;

		[SerializeField, HideInInspector, Tooltip("The title of the quest")]
		private LocalizedString title;

		[SerializeField, HideInInspector, Tooltip("The description of the quest")]
		private LocalizedString description;

		[SerializeField, HideInInspector]
		private string characterName = String.Empty;

		/// <summary>
		///
		/// </summary>
		[SerializeField, HideInInspector]
		private uint characterID = UInt32.MaxValue;

		[SerializeField, HideInInspector] // Tooltip("The character id where this story belongs to.")]
		private uint parentId = UInt32.MaxValue;

		[SerializeField, HideInInspector] // Tooltip("The id where the dialogue should go first")]
		private uint childId = UInt32.MaxValue;

		[SerializeField, Tooltip("Show the type of the quest. i.e could be part of the main story")]
		private QuestType typeId = QuestType.WorldQuests;

		public StorySO() : base("stories", "title", "parentId") { }

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
		}

		public void FinishStory()
		{
			m_IsDone = true;
		}

		private void Initialize()
		{
			if (childId != UInt32.MaxValue)
			{
				// Only get the first dialogue.
				startDialogue = DialogueLine.ConvertRow(TableDatabase.Get.GetRow("dialogues", childId),
					overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Dialogues"));

				var field = TableDatabase.Get.GetField(Name, "data", ID);
				if (field != null)
				{
					StoryTable.ParseNodeData(this, (JObject) field.Data);
				}
			}

			if (characterID != UInt32.MaxValue)
			{
				TableDatabase database = TableDatabase.Get;
				Tuple<uint, TableRow> link = database.FindLink("characters", "name", characterID);
				if (link != null)
				{
					var field = database.GetField(link.Item2, "name");
					if (field != null) characterName = (string) field.Data;

					Debug.Log(characterName);
				}
			}
		}
	}
}

