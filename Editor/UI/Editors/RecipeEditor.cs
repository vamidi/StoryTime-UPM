using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using StoryTime.Editor.Wizards;
using StoryTime.Components.ScriptableObjects;

namespace StoryTime.Editor.UI
{
	public class RecipeEditor : EditorTab<RecipeWizard, ItemRecipeSO>
	{
		internal new class UxmlFactory : UxmlFactory<RecipeEditor> {}

		public RecipeEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(RecipeEditor)}");
			asset.CloneTree(this);

			wizardButtonTitle = "Create Recipe/Craftable";
		}

		protected override void DrawSelection(Box cardInfo, ItemRecipeSO recipe)
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
