using System;
using UnityEngine;

using StoryTime.VisualScripting.Data.ScriptableObjects;
namespace StoryTime.Components.ScriptableObjects
{
	using VisualScripting.Data;

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

	[Serializable]
	internal class DialogueDetails
	{
		[SerializeField, Tooltip("The character associated with the story")] public CharacterSO[] speakers;
		// Is being calculated in the story editor.
		[SerializeField] public DialogueLine dialogue = new (true);
	}

	[CreateAssetMenu(fileName = "newSimpleStory", menuName = "StoryTime/Game/Narrative/Simple Story", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class SimpleStorySO : Graphable<StartNode>
	{
		public CharacterSO[] Speakers => dialogueDetails.speakers;

		public DialogueLine Dialogue => dialogueDetails.dialogue;

		[SerializeField] private DialogueDetails dialogueDetails = new();

		public SimpleStorySO() : base("stories", "title", "parentId") { }
	}
}
