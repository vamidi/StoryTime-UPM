using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public abstract class Node : ScriptableObject
	{
		public string guid;
		public Vector2 position;
		public List<Node> children;

		public void AddChild(Node parent, Node Child)
		{

		}

		public void RemoveChild(Node parent, Node Child)
		{

		}
	}
}
