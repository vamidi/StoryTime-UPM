using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Narrative.Dialogues
{
	using Utilities;
	using Attributes;
	using StoryTime.Domains.Utilities.Attributes;
	using StoryTime.Domains.Game.Characters.ScriptableObjects;
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
	
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
		public String ID
		{
			get => id;
			set => id = value;
		}

		public CharacterSO Speaker => speaker;

		public string SimplifiedSentence => simplifiedSentence;

		public LocalizedString Sentence => sentence;

		public DialogueType DialogueType => dialogueType;

		public bool isSimplified => simplified;

		public List<DialogueChoice> Choices => choices;

		[SerializeField, HideInInspector, Uuid] private String id;

		[SerializeField, Tooltip("Override If you want to display an text dialogue only.")]
		protected bool simplified;

		[SerializeField, ConditionalField(nameof(simplified)), TextArea(1, 25), Tooltip("Sentence that will showed when interacting")]
		protected string simplifiedSentence;

		[SerializeField] private DialogueType dialogueType;

		[SerializeField] private Emotion emotion;

		[SerializeField] protected CharacterSO speaker;

		[SerializeField, ConditionalField(nameof(simplified), inverse: true), Tooltip("Sentence that will showed when interacting")]
		private LocalizedString sentence;

		[SerializeField] private List<DialogueChoice> choices = new ();

		public DialogueLine(bool simplified = false)
		{
			this.simplified = simplified;
		}
	}
}
