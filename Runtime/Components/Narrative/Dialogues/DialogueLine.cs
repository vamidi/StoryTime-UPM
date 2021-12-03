using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Components
{
	using ScriptableObjects;

	/// <summary>
	/// DialogueLine is a Scriptable Object that represents one line of spoken dialogue.
	/// It references the Actor that speaks it.
	/// </summary>
	[Serializable]
	// ReSharper disable once InconsistentNaming
	public partial class DialogueLine
	{
		/// <summary>
		/// ID of the row inside the table.
		/// </summary>
		public UInt32 ID
		{
			get => id;
			set => id = value;
		}

		public LocalizedString CharacterName => characterName;

		public LocalizedString Sentence => sentence;
		public DialogueEventSO DialogueEvent
		{
			get => dialogueEvent;
			set => dialogueEvent =  value;
		}

		public DialogueType DialogueType => dialogueType;

		public DialogueLine NextDialogue
		{
			get => m_NextDialogue;
			set => m_NextDialogue = value;
		}

		public List<DialogueChoice> Choices => m_Choices;

		[SerializeField, HideInInspector] private UInt32 id = UInt32.MaxValue;

		/// <summary>
		/// Calculated through the node editor data.
		/// </summary>
		protected DialogueLine m_NextDialogue;

		[SerializeField, HideInInspector] private uint nextDialogueID = UInt32.MaxValue;

		[SerializeField] private DialogueType dialogueType;

		[SerializeField] protected LocalizedString characterName;

		/// <summary>
		///
		/// </summary>
		[SerializeField, Tooltip("Sentence that will showed when interacting")]
		private LocalizedString sentence;

		[SerializeField, Tooltip("Event that will be fired once filled in.")]
		private DialogueEventSO dialogueEvent = new DialogueEventSO("", null);

		private List<DialogueChoice> m_Choices = new List<DialogueChoice>();
	}
}
