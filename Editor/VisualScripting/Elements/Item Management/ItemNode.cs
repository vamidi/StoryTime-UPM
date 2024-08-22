
using UnityEditor.Experimental.GraphView;
using Node = StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node;

namespace StoryTime.Editor.VisualScripting.Elements
{
	using Utilities;

	public class ItemNode : NodeView
	{
		public ItemNode(ItemGraphView graphView, Node node) : base(graphView, node) { }

		public override void Draw()
		{
			title = node.name;

			// Output
			output = this.CreatePort("Item Connection");
			outputContainer.Add(output);

			RefreshExpandedState();
			RefreshPorts();
		}
	}
}
