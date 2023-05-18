using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using StoryTime.Editor.Wizards;
using StoryTime.Components.ScriptableObjects;
// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.UI
{
	public class SkillEditor : EditorTab<SkillWizard, SkillSO>
	{
		internal new class UxmlFactory : UxmlFactory<SkillEditor> {}

		public SkillEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(SkillEditor)}");
			asset.CloneTree(this);

			wizardButtonTitle = "Create Skill";
		}

		protected override void DrawSelection(Box cardInfo, SkillSO characterClass)
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
