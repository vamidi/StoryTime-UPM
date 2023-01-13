using StoryTime.Components;
using UnityEngine;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
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
