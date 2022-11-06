using System.Collections.Generic;
using StoryTime.Components;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public class DialogueNode : Node
	{
		public Content Content;
		// public NodeTypes Type;
		public List<DialogueChoice> Choices;
	}
}
