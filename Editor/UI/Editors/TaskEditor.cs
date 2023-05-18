using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using StoryTime.Editor.Wizards;
using StoryTime.Components.ScriptableObjects;
// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.UI
{
	public class TaskEditor : EditorTab<TaskWizard, TaskSO>
	{
		internal new class UxmlFactory : UxmlFactory<TaskEditor> {}

		public TaskEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(TaskEditor)}");
			asset.CloneTree(this);
		}

		protected override void DrawSelection(Box cardInfo, TaskSO task)
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
