using System;
using System.Collections.Generic;

using UnityEngine;

using UnityEditor;

namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Conversation", menuName = "StoryTime/Stories/Conversation", order = 0)]
	public class DialogueContainerSO : ScriptableObject
	{
		public StartNode rootNode;
		public List<Node> nodes = new ();

		public List<NodeLinkData> NodeLinks = new ();
		public List<NodeData> DialogueNodes = new ();
		public List<ExposedProperty> ExposedProperties = new ();

		public Node CreateNode(Type type)
		{
			Node node = CreateInstance(type) as Node;
			node.name = type.Name;
			node.guid = GUID.Generate().ToString();
			nodes.Add(node);

			AssetDatabase.AddObjectToAsset(node, this);
			AssetDatabase.SaveAssets();

			return node;
		}

		public void DeleteNode(Node node)
		{
			nodes.Remove(node);
			AssetDatabase.RemoveObjectFromAsset(node);
			AssetDatabase.SaveAssets();
		}
	}
}
