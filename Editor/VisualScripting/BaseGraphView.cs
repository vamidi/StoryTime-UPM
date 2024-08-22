using System;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using StoryTime.Domains.VisualScripting;
using StoryTime.Domains.VisualScripting.Data.Nodes.Dialogues;
using StoryTime.Domains.VisualScripting.Data.ScriptableObjects;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace StoryTime.Editor.VisualScripting
{
	using Elements;

	public abstract class BaseGraphView : GraphView
	{
		protected readonly NodeEditorService Service = new ();
		
		private readonly BaseGraphEditorWindow _editorWindow;
		private NodeSearchWindow _searchWindow;
		
		// TODO this is not the right place to store this variable
		protected NodeCollection nodes = new ();

		protected BaseGraphView()
		{
			AddGridBackground();
			AddSearchWindow();
		}

		public Group CreateGroup(string title, Vector2 mousePosition)
		{
			var group = new Group
			{
				title = title
			};
			group.SetPosition(new Rect(mousePosition, Vector2.zero));
			AddElement(group);
			return group;
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			var compatiblePorts = new List<Port>();
			ports.ForEach((port) =>
			{
				if (startPort == port) return;
				if (startPort.node == port.node) return;
				if (startPort.direction == port.direction) return;

				compatiblePorts.Add(port);
			});

			return compatiblePorts;
		}

		internal Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
		{
			var worldMousePosition = mousePosition;

			if (isSearchWindow) worldMousePosition -= _editorWindow.position.position;

			return contentViewContainer.WorldToLocal(worldMousePosition);
		}

		internal abstract void RemovePort(NodeView node, Port port);

		protected T FindNodeView<T>(StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node node)
			where T : Node
		{
			return GetNodeByGuid(node.guid) as T;
		}

		protected abstract NodeView CreateNodeView(StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node node,
			string title = "");

		private void AddGridBackground()
		{
			var grid = new GridBackground();
			grid.StretchToParentSize();
			Insert(0, grid);
		}

		private void AddSearchWindow()
		{
			if (_searchWindow == null)
			{
				_searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
				// TODO change me
				_searchWindow.Init(this, _editorWindow);
			}

			nodeCreationRequest = ctx => SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), _searchWindow);
		}
	}

	public abstract class BaseGraphView<T> : BaseGraphView
		where T : ScriptableObject
	{
		public Action<NodeView> OnNodeSelected;
		public Action<NodeView> OnNodeDeleted;

		public Blackboard BlackBoard;
		public List<ExposedProperty> ExposedProperties = new ();

		protected T container;
		protected TypeCache.TypeCollection types;

		protected readonly Vector2 DEFAULT_NODE_SIZE = new (350, 400);

		protected BaseGraphView()
		{
			Undo.undoRedoPerformed += OnUndoRedo;
		}

		public virtual void PopulateView(T viewContainer)
		{
			if (viewContainer == null)
			{
				return;
			}

			container = viewContainer;
			graphViewChanged -= OnGraphViewChanged;
			DeleteElements(graphElements);
			graphViewChanged += OnGraphViewChanged;

			GenerateEntryPointNode();
		}

		internal override void RemovePort(NodeView node, Port port)
		{
			var targetEdge = edges.ToList()
				.Where(x => x.output.portName == port.portName && x.output.node == port.node).ToArray();

			if (targetEdge.Any())
			{
				var edge = targetEdge.First();

				// Remove the child from the nodes
				NodeView parentView = edge.output.node as NodeView;
				NodeView childView = edge.input.node as NodeView;
				Service.RemoveChild(parentView.node, childView.node);

				// Remove link
				edge.input.Disconnect(edge);
				RemoveElement(edge);
			}

			if (node is DialogueNode dialogueNode)
			{
				if (dialogueNode.outputs.Count > 1)
				{
					dialogueNode.outputs.Remove(port);
				}
			}

			node.outputContainer.Remove(port);
			node.RefreshPorts();
			node.RefreshExpandedState();
		}

		protected abstract void GenerateEntryPointNode();

		protected abstract NodeView InitializeNodeView(StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node node, string title = "");

		protected virtual void InitStartNode(NodeView node)
		{
			node.capabilities &= ~Capabilities.Movable;
			node.capabilities &= ~Capabilities.Deletable;
		}

		protected override NodeView CreateNodeView(StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node node, string title = "")
		{
			NodeView nodeView = InitializeNodeView(node, title);

			nodeView.Draw();
			nodeView.RefreshExpandedState();
			nodeView.RefreshPorts();

			return nodeView;
		}

		private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
		{
			if (graphViewChange.elementsToRemove != null)
			{
				graphViewChange.elementsToRemove.ForEach(elem =>
				{
					if (elem is NodeView nodeView)
					{
						Service.DeleteNode(nodeView.node, ref nodes);
						OnNodeDeleted(null);
					}

					if (elem is Edge edge)
					{
						NodeView parentView = edge.output.node as NodeView;
						NodeView childView = edge.input.node as NodeView;
						Service.RemoveChild(parentView.node, childView.node);
					}
				});
			}

			if (graphViewChange.edgesToCreate != null)
			{
				graphViewChange.edgesToCreate.ForEach(edge =>
				{
					NodeView parentView = edge.output.node as NodeView;
					NodeView childView = edge.input.node as NodeView;
					Service.AddChild(parentView.node, childView.node);
				});
			}

			return graphViewChange;
		}

		protected IManipulator CreateNodeContextualMenu(string actionTitle, Type type)
		{
			var contextualMenuManipulator = new ContextualMenuManipulator(
				menuEvent => menuEvent.menu.AppendAction(
					actionTitle,
					actionEvent => AddElement(
						CreateNode(type, type.Name, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))
					)
				)
			);

			return contextualMenuManipulator;
		}

		private NodeView CreateNode(Type type, string title, Vector2 position)
		{
			return Instantiate(type, title, position);
		}

		private void OnUndoRedo()
		{
			PopulateView(container);
			AssetDatabase.SaveAssets();
		}

		private NodeView Instantiate(Type nodeType, string title = "", Vector2 position = new ())
		{
			if (nodeType == null)
			{
				Debug.LogWarning("Node class could not be found!");
				return null;
			}

			StoryTime.Domains.VisualScripting.Data.ScriptableObjects.Node node = Service.CreateNode(nodeType, ref nodes);
			node.position = position;

			return CreateNodeView(node, title);
		}
	}
}
