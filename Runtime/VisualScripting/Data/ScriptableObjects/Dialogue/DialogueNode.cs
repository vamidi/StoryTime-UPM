using System.Collections.Generic;
using UnityEngine;

using StoryTime.Components;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public interface IDialogueNode
	{
		public DialogueLine DialogueLine { get; }
	}

	public class DialogueNode : ChildNode, IDialogueNode
	{
		public DialogueLine DialogueLine
		{
			get => dialogueLine;
			internal set => dialogueLine = value;
		}

		DialogueLine IDialogueNode.DialogueLine => DialogueLine;

		public List<Node> Children => children;

		public List<DialogueChoice> Choices => choices;

		[SerializeField] private DialogueLine dialogueLine;
		// public NodeTypes Type;
		[SerializeField] private List<DialogueChoice> choices;

		[SerializeField] private List<Node> children;
	}
}
