
using StoryTime.VisualScripting.Data;
using StoryTime.Editor.VisualScripting.Elements;

namespace StoryTime.Editor.VisualScripting.Data
{
	public class DialogueNodeData : NodeData
	{
		public DialogueNodeData(DialogueNode node)
		{
			var content = new DialogContent();
			content.Fill(node);

			Guid = node.GUID;
			Content = content;
			Position = node.GetPosition().position;
			Type = node.DialogType;
			Choices = node.Choices;
		}
	}
}
