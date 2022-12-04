using UnityEditor.Experimental.GraphView;

using Node = StoryTime.VisualScripting.Data.ScriptableObjects.Node;
namespace StoryTime.Editor.VisualScripting.Elements
{
	using Utilities;

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
