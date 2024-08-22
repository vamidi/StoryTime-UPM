using UnityEngine;

using StoryTime.Components;
namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues
{
	public class StartNode : ChildNode, IDialogueNode
	{
		public DialogueLine DialogueLine
		{
			get => dialogueLine;
			internal set => dialogueLine = value;
		}

		DialogueLine IDialogueNode.DialogueLine => DialogueLine;

		[SerializeField] private DialogueLine dialogueLine;
	}
}
