using System.Collections.Generic;
using StoryTime.Components;
using UnityEngine;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
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

		DialogueLine IDialogueNode.DialogueLine
		{
			get => DialogueLine;
			set => DialogueLine = value;
		}

		[SerializeField] private Node child;
		[SerializeField] private DialogueLine dialogueLine;
	}
}
