using StoryTime.Components;
using UnityEngine;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public interface IDialogueNode
	{
		public DialogueLine DialogueLine { get; }
	}

	public class StartNode : Node, IDialogueNode
	{
		public Node Child
		{
			get => child;
			internal set => child = value;
		}

		internal DialogueLine DialogueLine
		{
			get => dialogueLine;
			set => dialogueLine = value;
		}

		DialogueLine IDialogueNode.DialogueLine => DialogueLine;

		public Node child;

		[SerializeField] private DialogueLine dialogueLine;
	}
}
