using UnityEditor.Experimental.GraphView;

namespace StoryTime.Editor.Domains.VisualScripting.Elements.ItemManagement
{
	using Utilities;
	using StoryTime.Editor.Domains.VisualScripting.ItemManagement;
	using Node = StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node;

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
