using UnityEngine;

namespace StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Dialogues
{
	using StoryTime.Domains.Narrative.Dialogues;

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
