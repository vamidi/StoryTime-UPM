using System;
using System.Linq;
using StoryTime.Domains.Game.Characters.ScriptableObjects;
using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.Editor.Wizards;
using StoryTime.Editor.Domains.UI;

// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.UI
{
	public class CharacterEditor : EditorTab<CharacterWizard, CharacterSO>
	{
		internal new class UxmlFactory : UxmlFactory<CharacterEditor> {}

		public CharacterEditor()
		{
			var asset = UIResourceHelper.GetTemplateAsset($"Editors/{nameof(CharacterEditor)}");
			asset.CloneTree(this);

			wizardButtonTitle = "Create Character";

			/*
			var t = target as Components.TableBehaviour;
			if (_choiceIndex >= 0)
			{
				var newID = m_PopulatedList.Keys.ToArray()[_choiceIndex];
				if (t && t.ID != newID)
				{
					// TODO split these two arrays
					// Update the selected choice in the underlying object
					t.ID = newID;

					OnChanged();

					// Save the changes back to the object
					// EditorUtility.SetDirty(target);
				}
			}
			*/
		}

		protected override void DrawSelection(Box cardInfo, CharacterSO character)
		{
			EditorGUI.BeginChangeCheck();

			serializedObject.UpdateIfRequiredOrScript();
			SerializedProperty iterator = serializedObject.GetIterator();
			for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
			{
				if (iterator.name != "id")
				{
					PropertyField prop = new PropertyField(iterator);
					prop.Bind(serializedObject);
					cardInfo.Add(prop);
				}
				else
				{
					Debug.Log(iterator.propertyPath);
					choiceIndex = Array.FindIndex(PopulatedList.Keys.ToArray(), idx => idx == character.ID);

					PopupField<String> popupField = new PopupField<String>("ID", PopulatedList.Keys.ToList(), choiceIndex)
					{
						bindingPath = "id",
						formatListItemCallback = i => PopulatedList[i],
						formatSelectedValueCallback = i => PopulatedList[i]
					};

					popupField.RegisterValueChangedCallback(evt =>
					{
						if (evt.newValue != "" && ItemListView.Selected != null)
						{
							if (ItemListView.Selected.ID != evt.newValue)
							{
								// TODO split these two arrays
								// Update the selected choice in the underlying object
								character.ID = evt.newValue;

								OnChanged();

								serializedObject.Update();
								serializedObject.ApplyModifiedProperties();

								EditorUtility.SetDirty(character);
								SceneView.RepaintAll();
							}
						}
					});

					popupField.BindProperty(serializedObject);
					cardInfo.Add(popupField);
				}
				using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
					EditorGUILayout.PropertyField(iterator, true);

			}
			serializedObject.ApplyModifiedProperties();
			EditorGUI.EndChangeCheck();

			/*
			SerializedProperty serializedProperty = serializedObject.GetIterator();
			serializedProperty.Next(true);

			while (serializedProperty.NextVisible(false))
			{
				PropertyField prop = new PropertyField(serializedProperty);
				prop.SetEnabled(serializedProperty.name != "m_Script");
				prop.Bind(serializedObject);
				cardInfo.Add(prop);
			}
			*/
		}

		protected override void OnChanged()
		{
			var t = ItemListView.Selected;
			if (t && t.ID != String.Empty)
			{
				var row = t.GetRow(t.TableName, t.ID);
				// set all the values from the selected row
				// if (row != null) t.ConvertRow(row, t);
			}
		}
	}
}
