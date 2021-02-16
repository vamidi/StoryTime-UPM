using System;
using System.Collections.Generic;
using UnityEngine;

namespace DatabaseSync.Components
{
	public enum DialogueType
	{
		StartDialogue,
		WinDialogue,
		LoseDialogue,
		DefaultDialogue,

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
	[CreateAssetMenu(fileName = "newStory", menuName = "DatabaseSync/Dialogues/Story")]
	// ReSharper disable once InconsistentNaming
	public class StorySO : TableBehaviour
	{
		[SerializeField]
		public uint ChildId = UInt32.MaxValue;

		[SerializeField]
		public string Description = String.Empty;

		[SerializeField]
		public uint ParentId = UInt32.MaxValue;

		[SerializeField]
		public string Title = String.Empty;

		[SerializeField] private ActorSO actor;
		[SerializeField] private List<DialogueLineSO> dialogueLines;
		[SerializeField] private DialogueType dialogueType;

		public ActorSO Actor => actor;

		public DialogueType DialogueType
		{
			get => dialogueType;
			set => dialogueType = value;
		}

		public List<DialogueLineSO> DialogueLines => dialogueLines;

		//TODO: Add support for branching conversations
		// Maybe add 2 (or more) special line slots which represent a choice in a conversation
		// Each line would also have an event associated, or another Dialogue
		public void AddDialogueLine(DialogueLineSO lineSo)
		{
			dialogueLines.Add(lineSo);
		}

		public StorySO(): base("stories", "title", "parentId") {}
	}
}

