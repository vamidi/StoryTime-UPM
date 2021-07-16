using System.Collections.Generic;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class RecipeEditor : EditorTab<Components.ItemRecipeSO>
	{
		internal new class UxmlFactory : UxmlFactory<RecipeEditor> {}

		public RecipeEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(RecipeEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
		}

		protected override void DrawSelection(Box cardInfo, Components.ItemRecipeSO recipe)
		{
			SerializedObject serializedObject = new SerializedObject(recipe);
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
