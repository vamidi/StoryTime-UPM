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

	[CreateAssetMenu(fileName = "newSimpleStory", menuName = "StoryTime/Stories/Simple Story", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class SimpleStorySO : Graphable<StartNode>
	{
		public CharacterSO Character => character;

		public DialogueLine Dialogue => dialogue;

		// TODO do we need this?
		[SerializeField, Tooltip("The character associated with the story")] protected CharacterSO character;

		// Is being calculated in the story editor.
		[SerializeField] protected DialogueLine dialogue = new (true);

		public SimpleStorySO() : base("stories", "title", "parentId") { }
	}
}
