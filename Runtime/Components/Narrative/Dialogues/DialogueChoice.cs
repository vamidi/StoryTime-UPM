using System;

using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace StoryTime.Components
{
	using Binary;
	using ScriptableObjects;
	// using ResourceManagement.Util;

	[Serializable]
	// ReSharper disable once InconsistentNaming
	public partial class DialogueChoice
	{
		/// <summary>
		/// ID of the row inside the table.
		/// </summary>
		public UInt32 ID
		{
			get => id;
			set => id = value;
		}

		public LocalizedString Sentence => text;

		public DialogueLine NextDialogue
		{
			get => nextDialogue;
			set => nextDialogue = value;
		}
		public string DialogueChoiceEvent => eventName;
		public ChoiceActionType ActionType => actionType;

		[SerializeField, HideInInspector] private UInt32 id = UInt32.MaxValue;

		/// <summary>
		/// The text we use to display.
		/// </summary>
		[SerializeField] private LocalizedString text;

		// This needs to be calculated
		[SerializeField] private DialogueLine nextDialogue;

		[SerializeField] private string eventName = String.Empty;

		[SerializeField] private ChoiceActionType actionType;

		public override string ToString()
		{
			// public uint NextDialogueID => nextDialogueID;
			// public List<DialogueOption> Choices => choices;
			return $"Choice, Sentence: {text}";
		}
	}
}
