using System;
using StoryTime.Domains.Attributes;
using UnityEngine;
using UnityEngine.Localization;
using Object = UnityEngine.Object;


#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace StoryTime.Components
{
	using ScriptableObjects;

	[Serializable]
	// ReSharper disable once InconsistentNaming
	public partial class DialogueChoice
	{
		/// <summary>
		/// ID of the row inside the table.
		/// </summary>
		public String ID
		{
			get => id;
			set => id = value;
		}

		public LocalizedString Sentence => text;

		public string DialogueChoiceEvent => eventName;
		public ChoiceActionType ActionType => actionType;

		[SerializeField, HideInInspector, Uuid] public String id;

		/// <summary>
		/// The text we use to display.
		/// </summary>
		[SerializeField] private LocalizedString text = new ();

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
