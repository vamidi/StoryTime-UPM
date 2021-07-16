using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class TaskEditor : EditorTab<Components.TaskSO>
	{
		internal new class UxmlFactory : UxmlFactory<TaskEditor> {}

		public TaskEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(TaskEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
		}

		protected override void DrawSelection(Box cardInfo, Components.TaskSO task)
		{
			SerializedObject serializedObject = new SerializedObject(task);
			SerializedProperty serializedProperty = serializedObject.GetIterator();
			serializedProperty.Next(true);

			while (serializedProperty.NextVisible(false))
			{
				PropertyField prop = new PropertyField(serializedProperty);
				prop.SetEnabled(serializedProperty.name != "m_Script");
				prop.Bind(serializedObject);
				cardInfo.Add(prop);
			}
		}
	}
}
