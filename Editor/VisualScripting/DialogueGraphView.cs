using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;

using StoryTime.VisualScripting.Data;
using StoryTime.VisualScripting.Data.ScriptableObjects;
using NodeSO = StoryTime.VisualScripting.Data.ScriptableObjects.Node;
using DialogueNodeSO = StoryTime.VisualScripting.Data.ScriptableObjects.DialogueNode;

namespace StoryTime.Editor.VisualScripting
{
	using Elements;
	using Utilities;

	public class DialogueGraphView : GraphView
	{
		public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }

		public Action<NodeView> OnNodeSelected;
		public DialogueContainerSO container;
		public Blackboard BlackBoard;
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
		}

		public void PopulateView(DialogueContainerSO dialogueContainer)
		{
			container = dialogueContainer;
			graphViewChanged -= OnGraphViewChanged;
			DeleteElements(graphElements);
			graphViewChanged += OnGraphViewChanged;

			AddElement(GenerateEntryPointNode());
			container.nodes.ForEach(n => AddElement(CreateNodeView(n, n.name)));
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
			var blackboardField = new BlackboardField{ text = property.PropertyName, typeText = "string"};
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

			var blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
			container.Add(blackBoardValueRow);

			BlackBoard.Add(container);
		}

		public NodeView CreateNode(NodeData nodeData)
		{
			return Instantiate(nodeData.Type);
		}

		public NodeView CreateNode(Type type, string title, Vector2 position)
		{
			return Instantiate(type, title, position);
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

		private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
		{
			if (graphViewChange.elementsToRemove != null)
			{
				graphViewChange.elementsToRemove.ForEach(elem =>
				{
					if (elem is NodeView nodeView)
					{
						container.DeleteNode(nodeView.node);
					}
				});
			}

			if (graphViewChange.edgesToCreate != null)
			{
				graphViewChange.edgesToCreate.ForEach(edge =>
				{
					NodeView parentView = edge.output.node as NodeView;
					NodeView childView = edge.output.node as NodeView;
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

			NodeSO node = container.CreateNode(nodeType);
			node.position = position;

			return CreateNodeView(node, title);
		}

		private NodeView CreateNodeView(NodeSO node, string title = "")
		{

			NodeView nodeView = new SingleChoiceNode(node)
			{
				title = title
			};
			nodeView.OnNodeSelected = OnNodeSelected;
			nodeView.Init(this, new Rect(node.position, DEFAULT_NODE_SIZE), node.GetType());
			nodeView.Draw();
			nodeView.RefreshExpandedState();
			nodeView.RefreshPorts();

			return nodeView;
		}

		private void AddManipulators()
		{
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			types = TypeCache.GetTypesDerivedFrom<NodeSO>();
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
				SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), _searchWindow);
		}

		private void AddGridBackground()
		{
			var grid = new GridBackground();
			// grid.StretchToParentSize();
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

		internal void RemovePort(DialogueNode node, Port socket)
		{
			if (node.Choices.Count > 1)
			{
				node.RemoveFromChoices(socket.portName);
				var targetEdge = edges.ToList()
					.Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
				if (targetEdge.Any())
				{
					var edge = targetEdge.First();
					edge.input.Disconnect(edge);
					RemoveElement(targetEdge.First());
				}

				node.outputContainer.Remove(socket);
				node.RefreshPorts();
				node.RefreshExpandedState();
			}
		}

		private NodeView GenerateEntryPointNode()
		{
			if (container.rootNode)
			{
				return CreateNodeView(container.rootNode, container.rootNode.name);
			}

			StartNode startNode = container.CreateNode(typeof(StartNode)) as StartNode;
			container.rootNode = startNode;
			var node = new DialogueNode(startNode)
			{
				title = "START",
				Content =
				{
					characterID = UInt32.MaxValue,
					dialogueText = "ENTRYPOINT"
				},
			};

			node.Init(this, new Rect(300, 200, 100, 150), startNode.GetType());

			var outputPort = node.CreatePort("Next");
			node.outputContainer.Add(outputPort);

			node.capabilities &= ~Capabilities.Movable;
			node.capabilities &= ~Capabilities.Deletable;

			node.Draw();
			node.RefreshExpandedState();
			node.RefreshPorts();

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

			return node;
		}
	}
}
