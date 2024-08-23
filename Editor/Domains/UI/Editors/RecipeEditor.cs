﻿using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StoryTime.Editor.Domains.UI.Editors
{
	using StoryTime.Editor.Domains.UI;
	using StoryTime.Editor.Domains.Wizards;
	using StoryTime.Domains.ItemManagement.Crafting.ScriptableObjects;
	
	public class RecipeEditor : EditorTab<RecipeWizard, ItemRecipeSO>
	{
		internal new class UxmlFactory : UxmlFactory<RecipeEditor> {}

		public RecipeEditor()
		{
			var asset = UIResourceHelper.GetTemplateAsset($"Editors/{nameof(RecipeEditor)}");
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