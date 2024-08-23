using UnityEditor.Experimental.GraphView;

namespace StoryTime.Editor.Domains.VisualScripting.Elements.ItemManagement
{
	using Utilities;
	using StoryTime.Editor.Domains.VisualScripting.ItemManagement;
	using Node = StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node;

	public class MasterNode : NodeView
	{
		public Port input;

		public MasterNode(ItemGraphView graphView, Node node) : base(graphView, node) { }

		public override void Draw()
		{
			title = node.name;

			// Input
			input = this.CreatePort("Result Connection", direction: Direction.Input,
				capacity: Port.Capacity.Multi);

			inputContainer.Add(input);

			RefreshExpandedState();
			RefreshPorts();
		}
	}
}
