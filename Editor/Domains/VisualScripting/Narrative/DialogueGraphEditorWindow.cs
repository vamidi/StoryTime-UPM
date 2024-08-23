
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
using StoryTime.Domains.VisualScripting.Data.Nodes.Dialogues;
using StoryTime.Editor.Domains.UI;

namespace StoryTime.Editor.Domains.VisualScripting.Narrative
{
	using Utilities;

	public class DialogueGraphEditorWindow : BaseGraphEditorWindow
	{
		private DialogueGraphView _graphView;

		private TextField _filenameTextField;
		private ObjectField _loadFileField;

		[MenuItem("Tools/StoryTime/Graph/Dialogue Graph")]
		public static void ShowWindow() => OpenDialogueGraphWindow();

		public static void OpenDialogueGraphWindow(StorySO container = null)
		{
			var window = GetWindow<DialogueGraphEditorWindow>();
			// TODO change to custom icon.
			window.titleContent = new GUIContent("Dialogue Graph",  EditorGUIUtility.IconContent("d_ScriptableObject Icon").image);
			window.LoadData(container);
			window.Show();
		}

		public void OnEnable()
		{
			// ConstructGraphView();
			// GenerateToolbar();
			// GenerateMiniMap();
			// GenerateBlackBoard();
			// GenerateSecondaryToolbar();
		}

		protected override void CreateGUI()
		{
			VisualElement root = rootVisualElement;

			var visualTree = UIResourceHelper.GetTemplateAsset($"VisualScripting/{nameof(DialogueGraphEditorWindow)}");
			visualTree.CloneTree(root);

			base.CreateGUI();

			// TODO change back to package uss version
			// var stylesheet = UI.Resources.GetStyleAsset($"VisualScripting/{nameof(DialogueGraphEditorWindow)}");
			// root.styleSheets.Add(stylesheet);

			// var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Templates/DialogueEditorWindow.uss");
			// root.styleSheets.Add(stylesheet);

			_graphView = root.Q<DialogueGraphView>();
			_graphView.OnNodeSelected = OnNodeSelectionChanged;

			OnSelectionChange();
		}

		private void OnSelectionChange()
		{
			StorySO container = Selection.activeObject as StorySO;
			if (container && AssetDatabase.CanOpenAssetInEditor(container.GetInstanceID()))
			{
				_graphView.PopulateView(container);
			}
		}

		private void GenerateMiniMap()
		{
			// var miniMap = new MiniMap{ anchored = true };
			// var coords = _graphView.contentViewContainer.WorldToLocal(new Vector2(maxSize.x - 10, 30));
			// miniMap.SetPosition(new Rect(coords.x, coords.y, 200, 140));
			// _graphView.Add(miniMap);
			var miniMap = new MiniMap { anchored = true };
			miniMap.SetPosition(new Rect(10, 55, 200, 140));
			_graphView.Add(miniMap);
		}

		private void GenerateBlackBoard()
		{
			var blackboard = new Blackboard(_graphView);
			blackboard.Add(new BlackboardSection{ title = "Exposed Properties" });
			blackboard.addItemRequested = blackBoard => _graphView.AddPropertyToBlackBoard(new ExposedProperty());
			blackboard.editTextRequested = (blackBoard, element, newValue) =>
			{
				var oldPropertyName = ((BlackboardField) element).text;
				if(_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
				{
					EditorUtility.DisplayDialog("Error",
						"This proeperty name already exists, please choose another one!", "OK");
					return;
				}

				var propertyIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
				_graphView.ExposedProperties[propertyIndex].PropertyName = newValue;
				((BlackboardField) element).text = newValue;
			};
			blackboard.SetPosition(new Rect(10, 30, 200, 300));
			_graphView.Add(blackboard);
			_graphView.BlackBoard = blackboard;
		}

		private void GenerateSecondaryToolbar()
		{
			var toolbar = new Toolbar();

			_loadFileField = ElementsUtilities.CreateObjectField<StorySO>("Load Graph");
			_loadFileField.objectType = typeof(StorySO);
			_loadFileField.RegisterCallback<ChangeEvent<StorySO>>((evt) =>
			{
				if (evt.newValue == null)
				{
					ResetTextFields();
					return;
				}

				_filename = evt.newValue.name;
				_filenameTextField.SetValueWithoutNotify(_filename);
				_filenameTextField.MarkDirtyRepaint();
				LoadData(evt.newValue);
			});

			toolbar.Add(_loadFileField);

			rootVisualElement.Insert(2, toolbar);
		}

		protected override void LoadGraph()
		{
			if (string.IsNullOrEmpty(_filename))
			{
				OnError();
				return;
			}

		}

		protected override void RequestSaveDataOperation()
		{
			if (string.IsNullOrEmpty(_filename))
			{
				OnError();
				return;
			}

			var saveUtility = GraphUtilities.Instance(_graphView);
			saveUtility.SaveGraph(_filename);
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
				var clearUtility = GraphUtilities.Instance(_graphView);
				clearUtility.ClearAll();
				ResetTextFields();
			}
		}

		private void ConstructGraphView()
		{
			/*
			_graphView = new DialogueGraphView(this)
			{
				name = "Dialogue Graph",
			};

			_graphView.StretchToParentSize();
			rootVisualElement.Add(_graphView);
			*/
		}

		private void SaveData()
		{
			if (string.IsNullOrEmpty(_filename))
			{
				OnError();
				return;
			}

			var saveUtility = GraphUtilities.Instance(_graphView);
			var previousGraph = (StorySO)_loadFileField.value;
			if (previousGraph != null)
			{
				// saveUtility.OverwriteGraph((DialogueContainer)loadFileField.value);
			}
			else
			{
				saveUtility.SaveGraph(_filename);
			}
		}

		private void LoadData(StorySO dialogContainer)
		{
			if (!dialogContainer)
			{
				return;
			}

			var loadUtility = GraphUtilities.Instance(_graphView);
			loadUtility.LoadGraph(dialogContainer);
		}

		private void ResetTextFields()
		{
			_loadFileField.value = null;
			_filename = TEXT;
			_filenameTextField.SetValueWithoutNotify(_filename);
			_filenameTextField.MarkDirtyRepaint();
		}
	}
}
