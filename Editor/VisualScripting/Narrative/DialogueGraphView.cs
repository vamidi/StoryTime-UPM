using System.Linq;
using System.Collections.Generic;

using UnityEngine.UIElements;

using UnityEditor;
using UnityEngine;

using StoryTime.VisualScripting.Data;
using StoryTime.Components.ScriptableObjects;
using StoryTime.VisualScripting.Data.ScriptableObjects;

using DialogueNodeSO = StoryTime.VisualScripting.Data.ScriptableObjects.DialogueNode;

namespace StoryTime.Editor.VisualScripting
{
	using Elements;
	using Utilities;

	public class DialogueGraphView : BaseGraphView<StorySO>
	{
		public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }

		public DialogueGraphView()
		{
			AddManipulators();
			AddStyles();
		}

		public override void PopulateView(StorySO dialogueContainer)
		{
			base.PopulateView(dialogueContainer);
			if (!dialogueContainer)
			{
				return;
			}

			// Creates node views
			container.nodes.ForEach(n => AddElement(CreateNodeView(n, n.name)));

			// Creates edges
			container.nodes.ForEach(n =>
			{
				if (n is DialogueNodeSO node)
				{
					// simple output only.
					if (node.Child)
					{
						DialogueNode parentView = FindNodeView<DialogueNode>(n);
						DialogueNode childView = FindNodeView<DialogueNode>(node.Child);

						// if the dialogue socket is connected. initialize it.
						UnityEditor.Experimental.GraphView.Edge edge = parentView.output.ConnectTo(childView.input);
						AddElement(edge);
					}

					var children = container.GetChildren(n);
					for (int i = 0; i < children.Count; i++)
					{
						Node child = children[i];
						DialogueNode parentView = FindNodeView<DialogueNode>(n);
						DialogueNode childView = FindNodeView<DialogueNode>(child);

						if (parentView.node is DialogueNodeSO parentDialogueNode)
						{
							if (parentDialogueNode.Choices.Count != parentView.outputs.Count)
								continue;

							UnityEditor.Experimental.GraphView.Edge edge = parentView.outputs[i].ConnectTo(childView.input);
							AddElement(edge);
						}
					}
				}
			});
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

		private void AddStyles()
		{
			var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Templates/DialogueEditorWindow.uss");
			styleSheets.Add(stylesheet);

			stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Templates/Variables.uss");
			styleSheets.Add(stylesheet);
		}

		private void AddManipulators()
		{
			SetupZoom(UnityEditor.Experimental.GraphView.ContentZoomer.DefaultMinScale,
				UnityEditor.Experimental.GraphView.ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new UnityEditor.Experimental.GraphView.ContentDragger());
			this.AddManipulator(new UnityEditor.Experimental.GraphView.SelectionDragger());
			this.AddManipulator(new UnityEditor.Experimental.GraphView.RectangleSelector());
			this.AddManipulator(CreateGroupContextualMenu());

			BuildMenu();

		}

		private void BuildMenu()
		{
			this.AddManipulator(CreateNodeContextualMenu($"[Dialogue]/{nameof(DialogueNodeSO)}", typeof(DialogueNodeSO)));
			this.AddManipulator(CreateNodeContextualMenu($"[Events]/Bool Events/{nameof(BoolEventNode)}", typeof(BoolEventNode)));
			this.AddManipulator(CreateNodeContextualMenu($"[Events]/Number Events/{nameof(IntEventNode)}", typeof(IntEventNode)));
			this.AddManipulator(CreateNodeContextualMenu($"[Events]/String Events/{nameof(StringEventNode)}", typeof(StringEventNode)));

			{
				types = TypeCache.GetTypesDerivedFrom<BoolEventNode>();
				foreach (var type in types) {
					this.AddManipulator(CreateNodeContextualMenu($"[Events]/Bool Events/{type.Name}", type));
				}
			}

			{
				types = TypeCache.GetTypesDerivedFrom<IntEventNode>();
				foreach (var type in types) {
					this.AddManipulator(CreateNodeContextualMenu($"[Events]/Number Events/{type.Name}", type));
				}
			}

			{
				types = TypeCache.GetTypesDerivedFrom<StringEventNode>();
				foreach (var type in types) {
					this.AddManipulator(CreateNodeContextualMenu($"[Events]/String Events/{type.Name}", type));
				}
			}

			{
				types = TypeCache.GetTypesDerivedFrom<DialogueNode>();
				foreach (var type in types) {
					this.AddManipulator(CreateNodeContextualMenu($"[Dialogue]/{type.Name}", type));
				}
			}
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

		protected override void GenerateEntryPointNode()
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
				OnNodeSelected = OnNodeSelected
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

		protected override NodeView InitializeNodeView(Node node, string title = "")
		{
			return new DialogueNode(this, node)
			{
				title = title,
				OnNodeSelected = OnNodeSelected
			};
		}

		protected override void InitStartNode(NodeView node)
		{
			base.InitStartNode(node);
			node.SetPosition(new Rect(100, 200, DEFAULT_NODE_SIZE.x, DEFAULT_NODE_SIZE.y));
		}

		protected override NodeView CreateNodeView(Node node, string title = "")
		{
			NodeView nodeView = base.CreateNodeView(node, title);
			if (node is StartNode)
			{
				InitStartNode(nodeView);
			}

			// See if we actually need this
			// nodeView.Draw();
			// nodeView.RefreshExpandedState();
			// nodeView.RefreshPorts();

			return nodeView;
		}
	}
}
