using System.Collections.Generic;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class EnemyEditor : EditorTab<Components.EnemySO>
	{
		internal new class UxmlFactory : UxmlFactory<EnemyEditor> {}

		public EnemyEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(EnemyEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
		}

		protected override void DrawSelection(Box cardInfo, Components.EnemySO enemy)
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
