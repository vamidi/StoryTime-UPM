using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class SkillEditor : EditorTab<Components.SkillSO>
	{
		internal new class UxmlFactory : UxmlFactory<SkillEditor> {}

		public SkillEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(SkillEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
		}

		protected override void DrawSelection(Box cardInfo, Components.SkillSO characterClass)
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
