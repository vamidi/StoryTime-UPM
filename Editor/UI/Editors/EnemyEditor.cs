using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using StoryTime.Editor.Wizards;
using StoryTime.Components.ScriptableObjects;

// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.UI
{
	public class EnemyEditor : EditorTab<EnemyWizard, EnemySO>
	{
		internal new class UxmlFactory : UxmlFactory<EnemyEditor> {}

		public EnemyEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(EnemyEditor)}");
			asset.CloneTree(this);
		}

		protected override void DrawSelection(Box cardInfo, EnemySO enemy)
		{
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
