using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StoryTime.Editor.VisualScripting
{
	using Elements;
	using Utilities;

	public abstract class BaseGraphEditorWindow : EditorWindow
	{
		protected InspectorView _inspectorView;

		protected string _filename = TEXT;
		protected const string TEXT = "New Graph";

		protected virtual void CreateGUI()
		{
			VisualElement root = rootVisualElement;
			_inspectorView = root.Q<InspectorView>();
		}

		private void GenerateToolbar()
		{
			var toolbar = new Toolbar();

			var fileNameTextField = new TextField("File Name");
			fileNameTextField.SetValueWithoutNotify(_filename);
			fileNameTextField.MarkDirtyRepaint();
			fileNameTextField.RegisterValueChangedCallback(evt => _filename = evt.newValue);
			toolbar.Add(fileNameTextField);

			toolbar.Add(ElementsUtilities.CreateButton("Save Data", RequestSaveDataOperation));
			toolbar.Add(ElementsUtilities.CreateButton( "Load Data", LoadGraph));
			toolbar.Add(ElementsUtilities.CreateButton("Clear All", ClearData));

			rootVisualElement.Add(toolbar);
		}

		protected void OnError()
		{
			EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid filename.", "OK");
		}

		protected void OnNodeSelectionChanged(NodeView obj)
		{
			_inspectorView.UpdateSelection(obj);
		}

		protected abstract void RequestSaveDataOperation();
		protected abstract void LoadGraph();
		protected abstract void ClearData();
	}
}
