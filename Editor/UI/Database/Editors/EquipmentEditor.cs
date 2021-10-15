using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class EquipmentEditor : EditorTab<Components.EquipmentSO>
	{
		internal new class UxmlFactory : UxmlFactory<EquipmentEditor> {}

		public EquipmentEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(EquipmentEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
		}

		protected override void DrawSelection(Box cardInfo, Components.EquipmentSO equipment)
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
