using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues
{
	using StoryTime.Domains.Narrative.Dialogues;
	
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
