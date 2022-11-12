using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;

using StoryTime.VisualScripting.Data;
using StoryTime.Components.ScriptableObjects;
using StoryTime.VisualScripting.Data.ScriptableObjects;
using DialogueNodeSO = StoryTime.VisualScripting.Data.ScriptableObjects.DialogueNode;

namespace StoryTime.Editor.VisualScripting
{
	using Elements;
	using Utilities;

	public class DialogueGraphView : UnityEditor.Experimental.GraphView.GraphView
	{
		public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }

		public Action<NodeView> OnNodeSelected;
		public Action<NodeView> OnNodeDeleted;
		public StorySO container;
		public UnityEditor.Experimental.GraphView.Blackboard BlackBoard;
		public List<ExposedProperty> ExposedProperties = new ();

		private readonly Vector2 DEFAULT_NODE_SIZE = new (350, 400);

		private readonly DialogueEditorWindow _editorWindow;
		private NodeSearchWindow _searchWindow;

		private TypeCache.TypeCollection types = new();

		/*
		public DialogueGraphView(DialogueEditorWindow editorWindow)
		{
			_editorWindow = editorWindow;
			AddStyles();


		}
		*/

		public DialogueGraphView()
		{
			AddGridBackground();
			AddSearchWindow();
			AddManipulators();

			var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Templates/DialogueEditorWindow.uss");
			styleSheets.Add(stylesheet);

			stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Templates/Variables.uss");
			styleSheets.Add(stylesheet);

			Undo.undoRedoPerformed += OnUndoRedo;
		}

		public void PopulateView(StorySO dialogueContainer)
		{
			if (!dialogueContainer)
			{
				return;
			}

			container = dialogueContainer;
			graphViewChanged -= OnGraphViewChanged;
			DeleteElements(graphElements);
			graphViewChanged += OnGraphViewChanged;

			// Creates node views
			container.nodes.ForEach(n => AddElement(CreateNodeView(n, n.name)));

			GenerateEntryPointNode();

			// Creates edges
			container.nodes.ForEach(n =>
			{
				var children = container.GetChildren(n);
				for (int i = 0; i < children.Count; i++)
				{
					Node child = children[i];
					DialogueNode parentView = FindNodeView(n);
					DialogueNode childView = FindNodeView(child);

					if (children.Count != parentView.outputs.Count)
						continue;

					UnityEditor.Experimental.GraphView.Edge edge = parentView.outputs[i].ConnectTo(childView.input);
					AddElement(edge);
				}
			});
		}

		public override List<UnityEditor.Experimental.GraphView.Port> GetCompatiblePorts(UnityEditor.Experimental.GraphView.Port startPort, UnityEditor.Experimental.GraphView.NodeAdapter nodeAdapter)
		{
			var compatiblePorts = new List<UnityEditor.Experimental.GraphView.Port>();
			ports.ForEach((port) =>
			{
				if (startPort == port) return;
				if (startPort.node == port.node) return;
				if (startPort.direction == port.direction) return;

				compatiblePorts.Add(port);
			});

			return compatiblePorts;
		}

		public void ClearBlackBoardAndExposedProperties()
		{
			ExposedProperties.Clear();
			BlackBoard.Clear();
		}

		public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
		{
			var localPropertyName = exposedProperty.PropertyName;
			var localPropertyValue = exposedProperty.PropertValue;

			while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
				localPropertyName = $"{localPropertyName}(1)";

			var property = new ExposedProperty
			{
				PropertyName = localPropertyName, PropertValue = localPropertyValue
			};
			ExposedProperties.Add(property);

			var container = new VisualElement();
			var blackboardField = new UnityEditor.Experimental.GraphView.BlackboardField{ text = property.PropertyName, typeText = "string"};
			container.Add(blackboardField);

			var propertyValueTextField = new TextField("Value:")
			{
				value = localPropertyName,
			};
			propertyValueTextField.RegisterValueChangedCallback(evt =>
			{
				var changingPropertyIndex = ExposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
				ExposedProperties[changingPropertyIndex].PropertValue = evt.newValue;
			});

			var blackBoardValueRow = new UnityEditor.Experimental.GraphView.BlackboardRow(blackboardField, propertyValueTextField);
			container.Add(blackBoardValueRow);

			BlackBoard.Add(container);
		}

		private void OnUndoRedo()
		{
			PopulateView(container);
			AssetDatabase.SaveAssets();
		}

		private NodeView CreateNode(Type type, string title, Vector2 position)
		{
			return Instantiate(type, title, position);
		}

		private DialogueNode FindNodeView(Node node)
		{
			return GetNodeByGuid(node.guid) as DialogueNode;
		}

		public UnityEditor.Experimental.GraphView.Group CreateGroup(string title, Vector2 mousePosition)
		{
			var group = new UnityEditor.Experimental.GraphView.Group
			{
				title = title
			};
			group.SetPosition(new Rect(mousePosition, Vector2.zero));
			AddElement(group);
			return group;
		}

		private UnityEditor.Experimental.GraphView.GraphViewChange OnGraphViewChanged(UnityEditor.Experimental.GraphView.GraphViewChange graphViewChange)
		{
			if (graphViewChange.elementsToRemove != null)
			{
				graphViewChange.elementsToRemove.ForEach(elem =>
				{
					if (elem is NodeView nodeView)
					{
						container.DeleteNode(nodeView.node);
						OnNodeDeleted(null);
					}

					if (elem is UnityEditor.Experimental.GraphView.Edge edge)
					{
						NodeView parentView = edge.output.node as NodeView;
						NodeView childView = edge.input.node as NodeView;
						container.RemoveChild(parentView.node, childView.node);
					}
				});
			}

			if (graphViewChange.edgesToCreate != null)
			{
				graphViewChange.edgesToCreate.ForEach(edge =>
				{
					NodeView parentView = edge.output.node as NodeView;
					NodeView childView = edge.input.node as NodeView;
					container.AddChild(parentView.node, childView.node);
				});
			}

			return graphViewChange;
		}

		private NodeView Instantiate(Type nodeType, string title = "", Vector2 position = new ())
		{
			if (nodeType == null)
			{
				Debug.LogWarning("Node class could not be found!");
				return null;
			}

			Node node = container.CreateNode(nodeType);
			node.position = position;

			return CreateNodeView(node, title);
		}

		private NodeView CreateNodeView(Node node, string title = "")
		{
			NodeView nodeView = new DialogueNode(this, node)
			{
				title = title
			};
			nodeView.OnNodeSelected = OnNodeSelected;

			if (node is StartNode)
			{
				InitStartNode(nodeView);
			}

			nodeView.Draw();
			nodeView.RefreshExpandedState();
			nodeView.RefreshPorts();

			return nodeView;
		}

		private void AddManipulators()
		{
			SetupZoom(UnityEditor.Experimental.GraphView.ContentZoomer.DefaultMinScale,
				UnityEditor.Experimental.GraphView.ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new UnityEditor.Experimental.GraphView.ContentDragger());
			this.AddManipulator(new UnityEditor.Experimental.GraphView.SelectionDragger());
			this.AddManipulator(new UnityEditor.Experimental.GraphView.RectangleSelector());

			types = TypeCache.GetTypesDerivedFrom<Node>();
			foreach(var type in types) {
				this.AddManipulator(CreateNodeContextualMenu($"[{type.BaseType.Name}] {type.Name}", type));
			}

			this.AddManipulator(CreateGroupContextualMenu());
		}

		private IManipulator CreateNodeContextualMenu(string actionTitle, Type type)
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

		private IManipulator CreateGroupContextualMenu()
		{
			var contextualMenuManipulator = new ContextualMenuManipulator(
				menuEvent => menuEvent.menu.AppendAction(
					"Create Group",
					actionEvent => AddElement(
						CreateGroup("Dialog Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))
					)
				)
			);

			return contextualMenuManipulator;
		}

		private void AddSearchWindow()
		{
			if (_searchWindow == null)
			{
				_searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
				_searchWindow.Init(this, _editorWindow);
			}

			nodeCreationRequest = ctx =>
				UnityEditor.Experimental.GraphView.SearchWindow.Open(new UnityEditor.Experimental.GraphView.SearchWindowContext(ctx.screenMousePosition), _searchWindow);
		}

		private void AddGridBackground()
		{
			var grid = new UnityEditor.Experimental.GraphView.GridBackground();
			grid.StretchToParentSize();
			Insert(0, grid);
		}

		private void AddStyles()
		{
			this.AddStyleSheets(
				Constants.STYLESHEET_GRAPH,
				Constants.STYLESHEET_NODE
			);
		}

		internal Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
		{
			var worldMousePosition = mousePosition;

			if (isSearchWindow) worldMousePosition -= _editorWindow.position.position;

			return contentViewContainer.WorldToLocal(worldMousePosition);
		}

		internal void RemovePort(DialogueNode node, UnityEditor.Experimental.GraphView.Port socket)
		{
			if (node.outputs.Count > 1)
			{
				var targetEdge = edges.ToList()
					.Where(x => x.output.portName == socket.portName && x.output.node == socket.node).ToArray();

				if (targetEdge.Any())
				{
					var edge = targetEdge.First();

					// Remove the child from the nodes
					NodeView parentView = edge.output.node as NodeView;
					NodeView childView = edge.input.node as NodeView;
					container.RemoveChild(parentView.node, childView.node);

					// Remove link
					edge.input.Disconnect(edge);
					RemoveElement(edge);
				}

				node.outputs.Remove(socket);
				node.outputContainer.Remove(socket);
				node.RefreshPorts();
				node.RefreshExpandedState();
			}
		}

		private void GenerateEntryPointNode()
		{
			if (container && container.rootNode)
			{
				return;
			}

			StartNode startNode = container.CreateNode(typeof(StartNode)) as StartNode;
			container.rootNode = startNode;
			var node = new DialogueNode(this, startNode)
			{
				title = "START",
			};

			var outputPort = node.CreatePort("Next");
			node.outputContainer.Add(outputPort);

			InitStartNode(node);

			node.Draw();
			node.RefreshExpandedState();
			node.RefreshPorts();

			EditorUtility.SetDirty(container);
			AssetDatabase.SaveAssets();

			/*
			var node = new DialogueNode
			{
				node = null,
				title = "START",
				DialogType = NodeTypes.Start,
				GUID = Guid.NewGuid().ToString(),
				EntryPoint = true

			};
			*/

			AddElement(node);
		}

		private void InitStartNode(NodeView node)
		{
			node.capabilities &= ~UnityEditor.Experimental.GraphView.Capabilities.Movable;
			node.capabilities &= ~UnityEditor.Experimental.GraphView.Capabilities.Deletable;
		}
	}
}
