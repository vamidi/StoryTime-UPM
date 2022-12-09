using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

using StoryTime.Utils.Attributes;
using StoryTime.Components.ScriptableObjects;
namespace StoryTime.VisualScripting.Data.ScriptableObjects
{
	public class Graphable<T> : LocalizationBehaviour, IGraphView
		where T : Node
	{
	public T Start => rootNode;

	// [Header("Graph editor variables")]
	[SerializeField, ConditionalField("isGraphEnabled")] internal T rootNode;
	[SerializeField, ConditionalField("isGraphEnabled")] internal List<Node> nodes = new();
	[SerializeField, ConditionalField("isGraphEnabled")] internal List<ExposedProperty> exposedProperties = new();

	public bool isGraphEnabled = true;

	public Graphable(string name, string dropdownColumn, string linkedColumn = "", uint linkedId = UInt32.MaxValue,
		string linkedTable = "")
		: base(name, dropdownColumn, linkedColumn, linkedId, linkedTable)
	{
	}

	public Node CreateNode(Type type)
	{
		Node node = CreateInstance(type) as Node;
		node.name = type.Name;
#if UNITY_EDITOR
		node.guid = GUID.Generate().ToString();
#endif
		Undo.RecordObject(this, "Dialogue Graphview (Create Node)");
		nodes.Add(node);

#if UNITY_EDITOR
		AssetDatabase.AddObjectToAsset(node, this);
		Undo.RegisterCreatedObjectUndo(node, "Dialogue Graphview (Create Node)");
		AssetDatabase.SaveAssets();
#endif
		return node;
	}

	public void DeleteNode(Node node)
	{
		Undo.RecordObject(this, "Dialogue Graphview (Create Node)");
		nodes.Remove(node);
#if UNITY_EDITOR
		// AssetDatabase.RemoveObjectFromAsset(node);
		Undo.DestroyObjectImmediate(node);
		AssetDatabase.SaveAssets();
#endif
	}

	public void AddChild(Node parent, Node child)
	{
		if (parent is StartNode startNode)
		{
			Undo.RecordObject(startNode, "Dialogue Graphview (Add Child)");
			startNode.Child = child;
			EditorUtility.SetDirty(startNode);
		}

		if (parent is EventNode eventNode)
		{
			Undo.RecordObject(eventNode, "Dialogue Graphview (Add Child)");
			eventNode.Child = child;
			EditorUtility.SetDirty(eventNode);
		}

		if (parent is DialogueNode dialogueNode && !dialogueNode.Children.Contains(child))
		{
			Undo.RecordObject(dialogueNode, "Dialogue Graphview (Add Child)");
			dialogueNode.Children.Add(child);
			EditorUtility.SetDirty(dialogueNode);
		}

		if (child is ItemMasterNode masterNode && parent is IngredientNode ingredientNode)
		{
			Undo.RecordObject(masterNode, "Item Graphview (Add Child)");
			masterNode.IngredientsList.Add(ingredientNode);
			EditorUtility.SetDirty(masterNode);
		}
	}

	public void RemoveChild(Node parent, Node child)
	{
		if (parent is StartNode startNode)
		{
			Undo.RecordObject(startNode, "Dialogue Graphview (Remove Child)");
			startNode.Child = null;
			EditorUtility.SetDirty(startNode);
		}

		if (parent is EventNode eventNode)
		{
			Undo.RecordObject(eventNode, "Dialogue Graphview (Add Child)");
			eventNode.Child = null;
			EditorUtility.SetDirty(eventNode);
		}

		if (parent is DialogueNode dialogueNode)
		{
			Undo.RecordObject(dialogueNode, "Dialogue Graphview (Remove Child)");
			dialogueNode.Children.Remove(child);
			EditorUtility.SetDirty(dialogueNode);
		}
	}

	public List<Node> GetChildren(Node parent)
	{
		List<Node> children = new();
		if (parent is DialogueNode dialogueNode)
		{
			children = dialogueNode.Children;
		}

		if (parent is StartNode startNode)
		{
			children.Add(startNode.child);
		}

		if (parent is ItemMasterNode masterNode)
		{
			foreach (var ingredientNode in masterNode.IngredientsList)
			{
				children.Add(ingredientNode);
			}
		}

		return children;
	}
	}
}
