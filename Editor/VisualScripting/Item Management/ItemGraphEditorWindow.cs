using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.Editor.Domains.UI;
using StoryTime.Domains.ItemManagement.Crafting.ScriptableObjects;

namespace StoryTime.Editor.VisualScripting
{
	public class ItemGraphEditorWindow : BaseGraphEditorWindow
	{
		private ItemGraphView _graphView;

		[MenuItem("Tools/StoryTime/Graph/Recipe Graph")]
		public static void ShowWindow() => OpenRecipeGraphWindow();

		public static void OpenRecipeGraphWindow(ItemRecipeSO container = null)
		{
			var window = GetWindow<ItemGraphEditorWindow>();
			// TODO change to custom icon.
			window.titleContent = new GUIContent("Item Recipe Graph",  EditorGUIUtility.IconContent("d_ScriptableObject Icon").image);
			window.LoadData(container);
			window.Show();
		}

		protected override void CreateGUI()
		{
			VisualElement root = rootVisualElement;

			var visualTree = UIResourceHelper.GetTemplateAsset($"VisualScripting/{nameof(ItemGraphEditorWindow)}");
			visualTree.CloneTree(root);

			base.CreateGUI();

			// TODO change back to package uss version
			// var stylesheet = UI.Resources.GetStyleAsset($"VisualScripting/{nameof(DialogueEditorWindow)}");
			// root.styleSheets.Add(stylesheet);

			// var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Templates/DialogueEditorWindow.uss");
			// root.styleSheets.Add(stylesheet);

			_graphView = root.Q<ItemGraphView>();
			_graphView.OnNodeSelected = OnNodeSelectionChanged;

			OnSelectionChange();
		}

		private void OnSelectionChange()
		{
			ItemRecipeSO container = Selection.activeObject as ItemRecipeSO;
			if (container && AssetDatabase.CanOpenAssetInEditor(container.GetInstanceID()))
			{
				_graphView.PopulateView(container);
			}
		}

		protected override void RequestSaveDataOperation()
		{
			if (string.IsNullOrEmpty(_filename))
			{
				OnError();
				return;
			}

			// var saveUtility = GraphUtilities.Instance(_graphView);
			// saveUtility.SaveGraph(_filename);
		}

		protected override void LoadGraph()
		{
			if (string.IsNullOrEmpty(_filename))
			{
				OnError();
				return;
			}
		}

		protected override void ClearData()
		{
			var choice = EditorUtility.DisplayDialogComplex(
				"Are you sure?",
				"This will clear everything. There's no turning back",
				"Yes",
				"Cancel",
				"");

			if (choice == 0)
			{
				// var clearUtility = GraphUtilities.Instance(_graphView);
				// clearUtility.ClearAll();
				// ResetTextFields();
			}
		}

		private void LoadData(ItemRecipeSO itemRecipeContainer)
		{
			if (!itemRecipeContainer)
			{
				return;
			}

			// var loadUtility = GraphUtilities.Instance(_graphView);
			// loadUtility.LoadGraph(itemRecipeContainer);
		}
	}
}
