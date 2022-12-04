using System.Collections.Generic;
using UnityEngine;

using StoryTime.Components;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public class DialogueNode : StartNode
	{
		public List<Node> Children => children;

		public List<DialogueChoice> Choices => choices;

		[SerializeField] private DialogueLine dialogueLine;
		// public NodeTypes Type;
		[SerializeField] private List<DialogueChoice> choices;

		[SerializeField] private List<Node> children;
	}
}
