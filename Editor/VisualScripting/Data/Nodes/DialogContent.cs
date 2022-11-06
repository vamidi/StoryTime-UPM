using System;

using StoryTime.VisualScripting.Data;
namespace StoryTime.Editor.VisualScripting.Data
{
	using Elements;

	[Serializable]
	public class DialogContent : Content
	{
		public void Fill(DialogueNode node)
		{
			characterID = node.Content.characterID;
			emotion = node.Content.emotion;
			dialogueText = node.Content.dialogueText;
		}
	}
}
