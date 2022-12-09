using System;
using System.Collections.Generic;

namespace StoryTime.VisualScripting
{
	using Data.ScriptableObjects;

	public interface IGraphView
	{
		Node CreateNode(Type type);
		void DeleteNode(Node node);

		void AddChild(Node parent, Node child);
		void RemoveChild(Node parent, Node child);
		public List<Node> GetChildren(Node parent);
	}
}
