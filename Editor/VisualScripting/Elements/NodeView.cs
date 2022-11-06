using System;

using UnityEngine;

using StoryTime.Editor.VisualScripting.Data;
using StoryTime.VisualScripting.Data.ScriptableObjects;

namespace StoryTime.Editor.VisualScripting.Elements
{
	public class NodeView : UnityEditor.Experimental.GraphView.Node
	{
		public Action<NodeView> OnNodeSelected;
		public Node node;

		public NodeView(Node node)
		{
			this.node = node;
			title = node.name;
			viewDataKey = node.guid;

			style.left = node.position.x;
			style.top = node.position.y;
		}

		public override void SetPosition(Rect newPos)
		{
			base.SetPosition(newPos);
			node.position = new Vector2(newPos.xMin, newPos.yMin);
		}

		public virtual void Init(DialogueGraphView graphView, DialogueNodeData nodeData, Type type) { }
		public virtual void Init(DialogueGraphView graphView, Rect position, Type type) { }
		public virtual void Draw() { }

		public override void OnSelected()
		{
			base.OnSelected();
			OnNodeSelected?.Invoke(this);
		}
	}
}
