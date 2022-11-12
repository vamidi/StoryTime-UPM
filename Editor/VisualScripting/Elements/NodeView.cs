using System;
using UnityEngine;

using UnityEditor;

using StoryTime.VisualScripting.Data.ScriptableObjects;
namespace StoryTime.Editor.VisualScripting.Elements
{
	public class NodeView : UnityEditor.Experimental.GraphView.Node
	{
		public Action<NodeView> OnNodeSelected;
		public readonly Node node;

		protected DialogueGraphView _graphView;

		public NodeView(DialogueGraphView graphView, Node node)
		{
			this.node = node;
			_graphView = graphView;
			title = node.name;
			viewDataKey = node.guid;

			style.left = node.position.x;
			style.top = node.position.y;

			mainContainer.AddToClassList("prata-node_main-container");
			extensionContainer.AddToClassList("prata-node_extension-container");
		}

		public override void SetPosition(Rect newPos)
		{
			base.SetPosition(newPos);
			Undo.RecordObject(node, "Dialogue GraphView (Set Position)");
			node.position = new Vector2(newPos.xMin, newPos.yMin);
			EditorUtility.SetDirty(node);
		}

		public virtual void Draw() { }

		public override void OnSelected()
		{
			base.OnSelected();
			OnNodeSelected?.Invoke(this);
		}
	}
}
