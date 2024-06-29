using UnityEngine;

using StoryTime.Utils.Attributes;
using StoryTime.VisualScripting.Data.ScriptableObjects;
namespace StoryTime.Components.ScriptableObjects
{
	// TODO Move to a separate file.
	public abstract class EnumType
	{
		public string Name {get; private set;}
		public string Description {get; private set;}
		
		protected EnumType(string name, string description) {
			Name = name;
			Description = description;
		}
	}
	
	public class StoryType: EnumType
	{
		public static readonly StoryType All = new ("All", "All stories");
		public static readonly StoryType WorldQuests = new ("WorldQuests", "World quest stories");
		public  readonly StoryType SideQuests = new ("SideQuests", "Side quest stories");
		// 	// Extend with own quest types.
		
		private StoryType(string name, string description) : base(name, description) { }
	}

	public class DialogueType: EnumType
	{
		public const string CStartDialogue = "StartDialogue";
		public const string CWinDialogue = "WinDialogue";
		public const string CLoseDialogue = "LoseDialogue";
		public const string CDefaultDialogue = "DefaultDialogue";
		
		public static readonly DialogueType StartDialogue = new ("StartDialogue", "Start dialogue");
		public static readonly DialogueType WinDialogue = new ("WinDialogue", "Win dialogue");
		public static readonly DialogueType LoseDialogue = new ("LoseDialogue", "Lose dialogue");
		public static readonly DialogueType DefaultDialogue = new ("DefaultDialogue", "Default dialogue");
		
		private DialogueType(string name, string description) : base(name, description) { }
	}

	public class ChoiceActionType: EnumType
	{
		public static readonly ChoiceActionType DoNothing = new ("DoNothing", "Do nothing");
		public static readonly ChoiceActionType ContinueWithQuest = new ("ContinueWithQuest", "Continue with quest");
		public static readonly ChoiceActionType ContinueWithTask = new ("ContinueWithTask", "Continue with task");
		
		private ChoiceActionType(string name, string description) : base(name, description) { }
	}

	[CreateAssetMenu(fileName = "NewSimpleStory", menuName = "StoryTime/Game/Narrative/Simple Story", order = 51)]
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
