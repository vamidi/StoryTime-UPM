using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace DatabaseSync.Components
{
	using Binary;
	using Attributes;

	[Serializable]
	public class DialogueLine : IDialogueLine
	{
		public UInt32 ID => m_Id;
		public LocalizedString Sentence => sentence;
		public DialogueEventSO DialogueEvent
		{
			get => dialogueEvent;
			set => dialogueEvent = value;
		}

		public DialogueType DialogueType => dialogueType;
		public IDialogueLine NextDialogue
		{
			get => m_NextDialogue;
			set
			{
				Debug.Assert(value is DialogueLine || value == null, "Expected to pass in a DialogueLine in class DialogueLine.");
				m_NextDialogue = (DialogueLine) value;
			}
		}

		public uint NextDialogueID => nextDialogueID;
		public List<DialogueChoiceSO> Choices => choices;

		private uint m_Id = UInt32.MaxValue;

		/// <summary>
		///
		/// </summary>
		private DialogueLine m_NextDialogue;

		[SerializeField, HideInInspector]
		private uint nextDialogueID = UInt32.MaxValue;

		[SerializeField]
		private DialogueType dialogueType;


		/// <summary>
		///
		/// </summary>
		[SerializeField, ConditionalField("overrideTableName"), Tooltip("Sentence that will showed when interacting")]
		private LocalizedString sentence;

		[SerializeField, Tooltip("Event that will be fired once filled in.")]
		private DialogueEventSO dialogueEvent = new DialogueEventSO("", null);

		// [NonSerialized]
		[SerializeField] private List<DialogueChoiceSO> choices = new List<DialogueChoiceSO>();

		public override string ToString()
		{
			// public uint NextDialogueID => nextDialogueID;
			// public List<DialogueOption> Choices => choices;
			return $"ID: {nextDialogueID}, Choices: {choices.Count}, Sentence: {sentence}";
		}

		public static DialogueLine ConvertRow(TableRow row, StringTableCollection collection, DialogueLine dialogueLine = null)
		{
			DialogueLine dialogue = dialogueLine ?? new DialogueLine();

			if (row.Fields.Count == 0)
			{
				return dialogue;
			}

			dialogue.m_Id = row.RowId;
			var entryId = (dialogue.ID + 1).ToString();
			if(collection)
				dialogue.sentence = new LocalizedString { TableReference = collection.TableCollectionNameReference, TableEntryReference = entryId };
			else
				Debug.LogError("Collection not found. Did you create any localization tables");

			foreach (var field in row.Fields)
			{
				if (field.Key.Equals("nextId"))
				{
					uint data = (uint) field.Value.Data;
					dialogue.nextDialogueID = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				}

				if (field.Key.Equals("options"))
				{
					// dialogue.sentence = (string) field.Value.Data;
				}

				// if (field.Key.Equals("parentId"))
				// {
				// uint data = (uint) field.Value.Data;
				// dialogue.parentId = data == UInt32.MaxValue - 1 ? UInt32.MaxValue : data;
				// }
			}

			return dialogue;
		}

		protected DialogueLine() { }

	}
}
