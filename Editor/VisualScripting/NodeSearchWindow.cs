using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;

using StoryTime.VisualScripting.Data;
using NodeSO = StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node;

namespace StoryTime.Editor.VisualScripting
{
	public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
	{
		private BaseGraphView _graphView;
		private Texture2D _indentationIcon;
		private TypeCache.TypeCollection types = new();

		public void Init(BaseGraphView graphView, EditorWindow window)
		{
			_graphView = graphView;
			_indentationIcon = new Texture2D(1, 1);
			_indentationIcon.SetPixel(0, 0, Color.clear);
			_indentationIcon.Apply();

			types = TypeCache.GetTypesDerivedFrom<NodeSO>();
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			var tree = new List<SearchTreeEntry>
			{
				new SearchTreeGroupEntry(new GUIContent("Create Elements:")),
				new SearchTreeGroupEntry(new GUIContent("Nodes"), 1),

				new(new GUIContent("Single Choice", _indentationIcon))
				{
					level = 2,
					userData = NodeTypes.SingleChoice
				},
				new(new GUIContent("Multiple Choice", _indentationIcon))
				{
					level = 2,
					userData = NodeTypes.MultipleChoice
				},
				new SearchTreeGroupEntry(new GUIContent("Dialog Group"), 1),
				new(new GUIContent("Single Group", _indentationIcon))
				{
					level = 2,
					userData = new Group()
				}
			};

			return tree;
		}

		public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
		{
			// var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,
				// context.screenMousePosition - _window.position.position);

			var localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

			switch (searchTreeEntry.userData)
			{
				case NodeTypes.SingleChoice:
					// _graphView.CreateNode(NodeTypes.SingleChoice, "Dialogue Node", localMousePosition);
					return true;
				case NodeTypes.MultipleChoice:
					// _graphView.CreateNode(NodeTypes.MultipleChoice, "Multiple choice Dialogue Node", localMousePosition);
					return true;
				case Group _:
					_graphView.CreateGroup("Dialog Group", localMousePosition);
					return true;
				default:
					return false;
			}
		}
	}
}

