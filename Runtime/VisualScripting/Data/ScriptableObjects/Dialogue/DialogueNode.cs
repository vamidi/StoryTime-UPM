using System.Collections.Generic;
using UnityEngine;

using StoryTime.Components;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public interface IDialogueNode
	{
		public DialogueLine DialogueLine { get; set; }
	}

	public class DialogueNode : Node, IDialogueNode
	{
		public List<Node> Children => children;

		internal DialogueLine DialogueLine
		{
			get => dialogueLine;
			set => dialogueLine = value;
		}

		DialogueLine IDialogueNode.DialogueLine
		{
			get => DialogueLine;
			set => DialogueLine = value;
		}

		public List<DialogueChoice> Choices => choices;

		[SerializeField] private DialogueLine dialogueLine;
		// public NodeTypes Type;
		[SerializeField] private List<DialogueChoice> choices;

		[SerializeField] private List<Node> children;
	}
}
