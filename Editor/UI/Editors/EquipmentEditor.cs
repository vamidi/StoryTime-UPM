using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using StoryTime.Editor.UI;
using StoryTime.Components.ScriptableObjects;

namespace StoryTime.Editor.UI
{
	public class EquipmentEditor : EditorTab<EquipmentSO>
	{
		internal new class UxmlFactory : UxmlFactory<EquipmentEditor> {}

		public EquipmentEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(EquipmentEditor)}");
			asset.CloneTree(this);
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
