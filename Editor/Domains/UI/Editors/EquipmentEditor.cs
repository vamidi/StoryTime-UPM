using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StoryTime.Editor.Domains.UI.Editors
{
	using StoryTime.Editor.Domains.Wizards;
	using StoryTime.Domains.ItemManagement.Equipment.ScriptableObjects;
	
	public class EquipmentEditor : EditorTab<EquipmentWizard, EquipmentSO>
	{
		internal new class UxmlFactory : UxmlFactory<EquipmentEditor> {}

		public EquipmentEditor()
		{
			var asset = UIResourceHelper.GetTemplateAsset($"Editors/{nameof(EquipmentEditor)}");
			asset.CloneTree(this);

			wizardButtonTitle = "Create Equipment";
		}

		protected override void DrawSelection(Box cardInfo, EquipmentSO equipment)
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
