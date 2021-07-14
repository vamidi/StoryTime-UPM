using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class CharacterEditor : EditorTab<Components.CharacterSO>
	{
		internal new class UxmlFactory : UxmlFactory<CharacterEditor> {}

		public CharacterEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(CharacterEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
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


		protected override void DrawSelection(Box cardInfo, Components.CharacterSO character)
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
					_choiceIndex = Array.FindIndex(m_PopulatedList.Keys.ToArray(), idx => idx == character.ID);

					PopupField<uint> popupField = new PopupField<uint>("ID", m_PopulatedList.Keys.ToList(), _choiceIndex)
					{
						bindingPath = "id",
						formatListItemCallback = i => m_PopulatedList[i],
						formatSelectedValueCallback = i => m_PopulatedList[i]
					};

					popupField.RegisterValueChangedCallback(evt =>
					{
						if (evt.newValue >= 0 && ItemListView.Selected != null)
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
			if (t && t.ID != UInt32.MaxValue)
			{
				var row = t.GetRow(t.Name, t.ID);
				// set all the values from the selected row
				if (row != null) Components.CharacterSO.CharacterTable.ConvertRow(row, t);
			}
		}
	}
}
