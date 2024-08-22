using StoryTime.Editor.VisualScripting.Utilities;
using UnityEditor;
using Node = StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node;

namespace StoryTime.Editor.VisualScripting.Elements
{
	public class StartNode : NodeView
	{
		public StartNode(BaseGraphView graphView, Node node) : base(graphView, node) { }

		public override void Draw()
		{
			base.Draw();

			var portChoice = this.CreatePort("Next Dialogue");
			output = portChoice;
			outputContainer.Add(portChoice);

			// keep track of undo records
			Undo.undoRedoPerformed += OnUndoRedo;
			RefreshExpandedState();
			RefreshPorts();
		}

		private void OnUndoRedo() { }
	}
}
