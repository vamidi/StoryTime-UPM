using System;
using UnityEngine;

using UnityEditor;

using StoryTime.Domains.VisualScripting.Data.ScriptableObjects;
namespace StoryTime.Editor.VisualScripting.Elements
{
	public class NodeView : UnityEditor.Experimental.GraphView.Node
	{
		public Action<NodeView> OnNodeSelected;
		public readonly Node node;

		public UnityEditor.Experimental.GraphView.Port output;

		private readonly BaseGraphView graphView;

		protected NodeView(BaseGraphView graphView, Node node)
		{
			this.node = node;
			this.graphView = graphView;
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

		public virtual void Draw()
		{
			title = node.name;
		}

		public override void OnSelected()
		{
			base.OnSelected();
			OnNodeSelected?.Invoke(this);
		}

		protected void RemovePort(UnityEditor.Experimental.GraphView.Port port)
		{
			graphView.RemovePort(this, port);
		}
	}
}
