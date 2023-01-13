
using UnityEngine.UIElements;

using UnityEditor;
using UnityEngine;

// using StoryTime.VisualScripting.Data;
// using StoryTime.Components.ScriptableObjects;
using StoryTime.VisualScripting.Data.ScriptableObjects;
using DialogueNodeSO = StoryTime.VisualScripting.Data.ScriptableObjects.DialogueNode;

namespace StoryTime.Editor.VisualScripting
{
	using Elements;
	using Components.ScriptableObjects;

	public class ItemGraphView : BaseGraphView<ItemRecipeSO>
	{
		public new class UxmlFactory : UxmlFactory<ItemGraphView, UxmlTraits> { }

		public ItemGraphView()
		{
			AddManipulators();
			AddStyles();
		}

		public override void PopulateView(ItemRecipeSO recipeContainer)
		{
			base.PopulateView(recipeContainer);
			if (!recipeContainer)
			{
				return;
			}

			// Creates node views
			container.nodes.Value.ForEach(n => AddElement(CreateNodeView(n, n.name)));

			// Creates edges
			container.nodes.Value.ForEach(n => {
				var children = container.GetChildren(n);
				children.ForEach(c => {
					MasterNode parentView = FindNodeView<MasterNode>(n);
					ItemNode childView = FindNodeView<ItemNode>(c);

					UnityEditor.Experimental.GraphView.Edge edge = parentView.input.ConnectTo(childView.output);
					AddElement(edge);
				});
			});
		}

		protected override void GenerateEntryPointNode()
		{
			if (container && container.rootNode)
			{
				return;
			}

			ItemMasterNode masterNode = container.CreateNode(typeof(ItemMasterNode)) as ItemMasterNode;
			container.rootNode = masterNode;
			var node = new MasterNode(this, masterNode)
			{
				title = "START",
				OnNodeSelected = OnNodeSelected
			};
			node.SetPosition(new Rect(350, 200, DEFAULT_NODE_SIZE.x, DEFAULT_NODE_SIZE.y));

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

			// AddElement(node);
		}

		protected override NodeView InitializeNodeView(Node node, string title = "")
		{
			if (node is ItemMasterNode)
			{
				return new MasterNode(this, node)
				{
					title = title,
					OnNodeSelected = OnNodeSelected
				};
			}

			return new ItemNode(this, node)
			{
				title = title,
				OnNodeSelected = OnNodeSelected
			};
		}

		protected override void InitStartNode(NodeView node)
		{
			base.InitStartNode(node);
			node.SetPosition(new Rect(350, 200, DEFAULT_NODE_SIZE.x, DEFAULT_NODE_SIZE.y));
		}

		protected override NodeView CreateNodeView(Node node, string title = "")
		{
			NodeView nodeView = base.CreateNodeView(node, title);
			if (node is ItemMasterNode)
			{
				InitStartNode(nodeView);
			}

			// See if we actually need this
			// nodeView.Draw();
			// nodeView.RefreshExpandedState();
			// nodeView.RefreshPorts();

			return nodeView;
		}

		private void AddManipulators()
		{
			SetupZoom(UnityEditor.Experimental.GraphView.ContentZoomer.DefaultMinScale,
				UnityEditor.Experimental.GraphView.ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new UnityEditor.Experimental.GraphView.ContentDragger());
			this.AddManipulator(new UnityEditor.Experimental.GraphView.SelectionDragger());
			this.AddManipulator(new UnityEditor.Experimental.GraphView.RectangleSelector());

			BuildMenu();
		}

		private void BuildMenu()
		{
			this.AddManipulator(CreateNodeContextualMenu($"[Items]/{nameof(IngredientNode)}", typeof(IngredientNode)));

			{
				types = TypeCache.GetTypesDerivedFrom<IngredientNode>();
				foreach (var type in types) {
					this.AddManipulator(CreateNodeContextualMenu($"[Items/{type.Name}", type));
				}
			}
		}

		private void AddStyles()
		{
			var stylesheet = UI.Resources.GetStyleAsset("VisualScripting/DialogueGraphEditorWindow");

			styleSheets.Add(stylesheet);

			stylesheet = UI.Resources.GetStyleAsset("VisualScripting/Variables");
			styleSheets.Add(stylesheet);
		}
	}
}
