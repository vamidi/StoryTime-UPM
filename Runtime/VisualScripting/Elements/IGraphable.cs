using System;
using System.Collections.Generic;

namespace StoryTime.VisualScripting
{
	using Data.ScriptableObjects;

	public interface IGraphView
	{
		Node CreateNode(Type type, ref NodeCollection nodes);
		void DeleteNode(Node node, ref NodeCollection nodes);

		void AddChild(Node parent, Node child);
		void RemoveChild(Node parent, Node child);
		public List<Node> GetChildren(Node parent);
	}
}
