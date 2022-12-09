using System;
using System.Collections.Generic;
using StoryTime.Utils.Attributes;
using UnityEditor.Localization;
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

		public DialogueType DialogueType => dialogueType;

		public List<DialogueChoice> Choices => choices;

		[SerializeField, HideInInspector] private UInt32 id = UInt32.MaxValue;

		[SerializeField, Tooltip("Override If you want to display an text dialogue only.")]
		protected bool simplified;

		[SerializeField, ConditionalField("simplified"), TextArea(1, 25)]
		protected string simplifiedSentence;

		[SerializeField] private DialogueType dialogueType;

		[SerializeField] private Emotion emotion;

		[SerializeField] protected LocalizedString characterName;

		[SerializeField, Tooltip("Sentence that will showed when interacting")]
		private LocalizedString sentence;

		[SerializeField] private List<DialogueChoice> choices = new ();

		public DialogueLine(bool simplified = false)
		{
			this.simplified = simplified;
		}
	}
}
