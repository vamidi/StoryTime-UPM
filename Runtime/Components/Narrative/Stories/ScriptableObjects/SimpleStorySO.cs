using UnityEngine;

using StoryTime.Utils.Attributes;
using StoryTime.VisualScripting.Data.ScriptableObjects;
namespace StoryTime.Components.ScriptableObjects
{
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

	[CreateAssetMenu(fileName = "newSimpleStory", menuName = "StoryTime/Game/Narrative/Simple Story", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class SimpleStorySO : Graphable<StartNode>
	{
		public CharacterSO Character => character;

		public DialogueLine Dialogue => dialogue;

		[SerializeField, Tooltip("The character associated with the story")] public CharacterSO character;

		// Is being calculated in the story editor.
		[SerializeField, ConditionalField(nameof(isGraphEnabled), inverse: true)] public DialogueLine dialogue = new (true);

		public SimpleStorySO() : base("stories", "title", "parentId")
		{
			isGraphEnabled = false;
		}
	}
}
