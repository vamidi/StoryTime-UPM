using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class ClassEditor : EditorTab<Components.CharacterClassSO>
	{
		internal new class UxmlFactory : UxmlFactory<ClassEditor> {}

		public ClassEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(ClassEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
		}

		protected override void DrawSelection(Box cardInfo, Components.CharacterClassSO characterClass)
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
