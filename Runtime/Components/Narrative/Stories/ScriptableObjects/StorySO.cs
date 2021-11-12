using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace StoryTime.Components.ScriptableObjects
{
	using Database;

	/// <summary>
	/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
	/// In future versions it might contain support for branching conversations.
	/// </summary>
	[CreateAssetMenu(fileName = "newStory", menuName = "StoryTime/Stories/Story", order = 51)]
	// ReSharper disable once InconsistentNaming
	public partial class StorySO : SimpleStorySO
	{
		public LocalizedString Title => title;
		public LocalizedString Description => description;

		public StoryType TypeId => typeId;
		public List<TaskSO> Tasks => tasks;

		[SerializeField, Tooltip("The collection of tasks composing the Quest")] private List<TaskSO> tasks = new List<TaskSO>();

		[SerializeField, HideInInspector, Tooltip("The title of the quest")]
		private LocalizedString title;

		[SerializeField, HideInInspector, Tooltip("The description of the quest")]
		private LocalizedString description;

		[SerializeField, Tooltip("Show the type of the quest. i.e could be part of the main story")]
		private StoryType typeId = StoryType.WorldQuests;

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
			if (ID != UInt32.MaxValue && childId != UInt32.MaxValue)
			{
				dialogueLines.Clear();
				// Only get the first dialogue.
				dialogueLines.Add(
					DialogueLine.DialogueTable.ConvertRow(TableDatabase.Get.GetRow("dialogues", childId),
						overrideTable ? collection : LocalizationEditorSettings.GetStringTableCollection("Dialogues"),
						overrideCharacterTable ? characterCollection : LocalizationEditorSettings.GetStringTableCollection("Character Names"))
				);

				var field = TableDatabase.Get.GetField(Name, "data", ID);
				if (field != null)
				{
					StoryTable.ParseNodeData(this, (JObject) field.Data);
				}
			}
		}
	}
}

