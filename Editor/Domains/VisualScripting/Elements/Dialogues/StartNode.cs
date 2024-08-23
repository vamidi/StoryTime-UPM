using UnityEditor;

namespace StoryTime.Editor.Domains.VisualScripting.Elements.Dialogues
{
	using Utilities;
	using Node = StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node;
	
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
