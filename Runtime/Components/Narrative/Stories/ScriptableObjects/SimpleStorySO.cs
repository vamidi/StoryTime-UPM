using System;

using UnityEditor.Localization;
using UnityEngine;

namespace DatabaseSync.Components
{
	using Attributes;

	public enum StoryType
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

	[CreateAssetMenu(fileName = "newSimpleStory", menuName = "DatabaseSync/Stories/Simple Story", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class SimpleStorySO : LocalizationBehaviour
	{
		public string CharacterName => characterName;
		public CharacterSO Character => character;

		// rename Dialogue line to story lines
		public DialogueLine StartDialogue => startDialogue;

		public bool IsDone => m_IsDone;

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		public uint ParentId => parentId;
		public uint ChildId => childId;

		[SerializeField, Tooltip("Override where we should get the dialogue options data from.")]
		protected bool overrideDialogueOptionsTable;

		[SerializeField, ConditionalField("overrideDialogueOptionsTable"), Tooltip("Table collection we are going to use for the sentence")]
		protected StringTableCollection dialogueOptionsCollection;

		[SerializeField] protected CharacterSO character;

		[SerializeField, HideInInspector]
		protected string characterName = String.Empty;

		// Is being calculated in the story editor.
		[SerializeField] protected DialogueLine startDialogue;

		/** ------------------------------ DATABASE FIELD ------------------------------ */

		/// <summary>
		///
		/// </summary>
		[SerializeField, HideInInspector]
		protected uint characterID = UInt32.MaxValue;

		[SerializeField, HideInInspector] // Tooltip("The character id where this story belongs to.")]
		protected uint parentId = UInt32.MaxValue;

		[SerializeField, HideInInspector] // Tooltip("The id where the dialogue should go first")]
		protected uint childId = UInt32.MaxValue;

		// ReSharper disable once InconsistentNaming
		protected bool m_IsDone;

		public SimpleStorySO() : base("stories", "title", "parentId") { }

		public override void Reset()
		{
			base.Reset();

			childId = UInt32.MaxValue;

			Debug.Log(childId);
		}
	}
}
