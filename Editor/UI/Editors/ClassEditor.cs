using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine.UIElements;

using StoryTime.Components.ScriptableObjects;
namespace StoryTime.Editor.UI
{
	public class ClassEditor : EditorTab<CharacterClassSO>
	{
		internal new class UxmlFactory : UxmlFactory<ClassEditor> {}

		public ClassEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(ClassEditor)}");
			asset.CloneTree(this);
		}

		protected override void DrawSelection(Box cardInfo, CharacterClassSO characterClass)
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
