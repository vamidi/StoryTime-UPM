using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StoryTime.Editor.Domains.UI.Editors
{
	using StoryTime.Editor.Domains.UI;
	using StoryTime.Editor.Domains.Wizards;
	using StoryTime.Domains.Game.Characters.ScriptableObjects;
	
	public class SkillEditor : EditorTab<SkillWizard, SkillSO>
	{
		internal new class UxmlFactory : UxmlFactory<SkillEditor> {}

		public SkillEditor()
		{
			var asset = UIResourceHelper.GetTemplateAsset($"Editors/{nameof(SkillEditor)}");
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
