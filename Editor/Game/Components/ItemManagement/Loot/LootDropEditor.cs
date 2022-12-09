
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditorInternal;

using StoryTime.Components;
using StoryTime.Utils.Extensions;
using StoryTime.Components.ScriptableObjects;

// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.Components
{
	[CustomEditor(typeof(LootTable))]
	public class LootDropEditor : UnityEditor.Editor
	{
		private SerializedProperty _guaranteedList;
		private ReorderableList _reorderableGuaranteed;

		// Change
		private SerializedProperty _changeToGetList;
		private ReorderableList _reorderableChange;

		public override void OnInspectorGUI()
		{
			EditorUtility.SetDirty(target);
			LootTable loot = (LootTable) target;

			EditorGUILayout.BeginVertical("box");
			EditorGUI.indentLevel = 0;

			// Loot Name
			GUIStyle myStyle = new GUIStyle();
			myStyle.normal.textColor = GUI.color;
			myStyle.alignment = TextAnchor.UpperCenter;
			myStyle.fontStyle = FontStyle.Bold;
			// int _ti = myStyle.fontSize;

			EditorGUILayout.LabelField($"Loot Table", myStyle);

			myStyle.fontStyle = FontStyle.Italic;
			myStyle.fontSize = 20;

			EditorGUILayout.LabelField($",,{loot.name}''", myStyle);

			EditorGUILayout.Space(10);

			EditorGUI.BeginChangeCheck();

			_reorderableGuaranteed.DoLayoutList();
			_reorderableChange.DoLayoutList();

			if (EditorGUI.EndChangeCheck())
			{
				// loot.GuaranteedLootTable
				AssignLootItemSerializedProperty(_reorderableGuaranteed, loot.guaranteedLootTable);

				// loot.OneItemFromList
				AssignLootItemSerializedProperty(_reorderableChange, loot.oneItemFromList);
			}

			// Nothing Weight
			loot.weightToNoDrop = EditorGUILayout.FloatField("No Drop Weight", loot.weightToNoDrop);

			EditorGUILayout.EndVertical();

			EditorGUILayout.Space();

			Rect r = EditorGUILayout.BeginVertical("box");
			myStyle.fontStyle = FontStyle.Bold;
			myStyle.fontSize = 20;

			EditorGUILayout.Space(5);
			EditorGUILayout.LabelField($"Drop Change", myStyle);

			float totalWeight = loot.weightToNoDrop;
			float guaranteedHeight = 0;

			if (loot.oneItemFromList != null)
			{
				for (int j = 0; j < loot.oneItemFromList.Count; j++)
				{
					totalWeight += loot.oneItemFromList[j].Weight;
				}
			}

			if (0 < loot.guaranteedLootTable.Count)
			{
				guaranteedHeight += 10;
			}

			/* Guaranteed */
			for (int i = 0; i < loot.guaranteedLootTable.Count; i++)
			{
				string title = "";
				guaranteedHeight += 25;

				if(loot.guaranteedLootTable[i].Item != null && loot.guaranteedLootTable[i].Item.Prefab != null)
				{
					title = loot.guaranteedLootTable[i].Item.Prefab.name;
				}
				else
				{
					title = " --- No Drop Object --- ";
				}

				EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i), r.width - 10, 20), 1,
					$"{title} [{loot.guaranteedLootTable[i].MinCountItem}-{loot.guaranteedLootTable[i].MaxCountItem}]   -   Guaranteed");
			}

			/* Not Guaranteed */
			if (loot.oneItemFromList != null)
			{
				for (int i = 0; i < loot.oneItemFromList.Count; i++)
				{
					string title = "";
					if(loot.oneItemFromList[i].Item != null && loot.oneItemFromList[i].Item.Prefab != null)
					{
						title = loot.oneItemFromList[i].Item.Prefab.name;
					}
					else
					{
						title = "!!! No Drop Object Attachment !!!";
					}

					EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight, r.width - 10, 20),
						loot.oneItemFromList[i].Weight / totalWeight,
						$"{title} [{loot.oneItemFromList[i].MinCountItem}-{loot.oneItemFromList[i].MaxCountItem}]   -   {(loot.oneItemFromList[i].Weight / totalWeight * 100):f2}%");
				}
			}

			EditorGUI.ProgressBar(
				new Rect(r.x + 5, r.y + 40 + (25 * loot.oneItemFromList.Count + 10) + guaranteedHeight, r.width - 10,
					20), loot.weightToNoDrop / totalWeight,
				$"Nothing Additional   -   {(loot.weightToNoDrop / totalWeight * 100):f2}%");

			EditorGUILayout.Space(25 * loot.oneItemFromList.Count + 45 + guaranteedHeight);

			EditorGUILayout.EndVertical();
		}

		protected void OnEnable()
		{
			/* GUARANTEED */
			_guaranteedList = serializedObject.FindProperty("guaranteedLootTable");
			_reorderableGuaranteed = new ReorderableList(serializedObject, _guaranteedList, true, true, true, true);
			// Functions
			_reorderableGuaranteed.drawElementCallback = DrawGuaranteedListItems;
			_reorderableGuaranteed.drawHeaderCallback = DrawHeaderGuaranteed;

			/* Change */
			_changeToGetList = serializedObject.FindProperty("oneItemFromList");
			_reorderableChange = new ReorderableList(serializedObject, _changeToGetList, true, true, true, true);

			// Functions
			_reorderableChange.drawElementCallback += DrawChangeListItems;
			_reorderableChange.drawHeaderCallback += DrawHeaderChange;
		}

		private void DrawHeaderGuaranteed(Rect rect) { EditorGUI.LabelField(rect, "Guaranteed Loot Table"); }
		private void DrawHeaderChange(Rect rect) { EditorGUI.LabelField(rect, "Change To Get Loot Table"); }

		void DrawGuaranteedListItems(Rect rect, int index, bool isActive, bool isFocused)
		{
			LootTable loot = (LootTable)target;
			_reorderableGuaranteed.elementHeight = 82;
			DrawListItem(_reorderableGuaranteed, rect, index, isActive, isFocused);
		}

		void DrawChangeListItems(Rect rect, int index, bool isActive, bool isFocused)
		{
			_reorderableChange.elementHeight = 82;
			DrawListItem(_reorderableChange, rect, index, isActive, isFocused);
		}

		private void DrawListItem(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
			if (element != null)
			{
				EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, GUIContent.none);
			}
		}

		private void AssignLootItemSerializedProperty(ReorderableList list, List<DropItemStack> stack)
		{
			if (stack.Count == 0) {
				return;
			}

			for (int index = 0; index < stack.Count; index++)
			{
				// non-, Guaranteed
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				if (element != null)
				{
					stack[index].Weight = element.FindPropertyRelative("weight").floatValue;
					stack[index].Item = (ItemSO) element.FindPropertyRelative("item").objectReferenceValue;
					stack[index].MinCountItem = element.FindPropertyRelative("minCountItem").intValue;
					stack[index].MaxCountItem = element.FindPropertyRelative("maxCountItem").intValue;
				}
			}
		}

		private void DrawProgressbar(List<DropItemStack> list, ref float guaranteedHeight, bool increaseHeight = false)
		{
			Rect r = EditorGUILayout.BeginVertical("box");
			for (int i = 0; i < list.Count; i++)
			{
				if (increaseHeight) {
					guaranteedHeight += 25;
				}

				var stack = list[i];
				// var title = stack.Item != null && stack.Item.Prefab ? stack.Item.Prefab.name : " --- No Drop Object --- ";

				//EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i), r.width - 10, 20), 1,
					// $"{title} [{stack.MinCountItem}-{stack.MaxCountItem}]   -   Guaranteed");
			}
		}
	}

	/// <summary>
    /// Custom Property Draw
    /// </summary>
    [CustomPropertyDrawer(typeof(DropItemStack))]
    public class DropChangeItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var weightRectLabel = new Rect(position.x, position.y, 55, 18);
            var weightRect = new Rect(position.x, position.y + 20, 55, 18);

            EditorGUI.LabelField(weightRectLabel, "weight");
            EditorGUI.PropertyField(weightRect, property.FindPropertyRelative("weight"), GUIContent.none);

            var ItemRectLabel = new Rect(position.x + 60, position.y, position.width - 60, 18);
            var ItemRect = new Rect(position.x + 60, position.y + 20, position.width - 60 - 75, 18);

            var itemProp = property.FindPropertyRelative("item");
            EditorGUI.LabelField(ItemRectLabel, "Item");
            EditorGUI.PropertyField(ItemRect, itemProp, GUIContent.none);

            var MinMaxRectLabel = new Rect(position.x + position.width - 70, position.y, 70, 18);

            var MinRect = new Rect(position.x + position.width - 70, position.y + 20, 30, 18);
            var MinMaxRect = new Rect(position.x + position.width - 39, position.y + 20, 9, 18);
            var MaxRect = new Rect(position.x + position.width - 30, position.y + 20, 30, 18);

            EditorGUI.LabelField(MinMaxRectLabel, "Min  -  Max");
            EditorGUI.PropertyField(MinRect, property.FindPropertyRelative("minCountItem"), GUIContent.none);
            EditorGUI.LabelField(MinMaxRect, "-");
            EditorGUI.PropertyField(MaxRect, property.FindPropertyRelative("maxCountItem"), GUIContent.none);

            if (itemProp != null)
            {
	            ItemSO itemRef = (ItemSO) itemProp.objectReferenceValue;
	            if (itemRef)
	            {
		            SerializedObject serializedObject = new SerializedObject(itemRef);
		            var prefabProp = serializedObject.FindProperty("itemSettings.prefab");

		            using (new EditorGUI.DisabledGroupScope(prefabProp == null))
		            {
			            if (prefabProp != null)
			            {
				            EditorGUI.LabelField(new Rect(position.x, position.y + 40, 55, 18), "Item drop");
				            EditorGUI.PropertyField(new Rect(position.x + 60, position.y + 40, position.width - 60 - 75, 18), prefabProp, GUIContent.none);
				            serializedObject.ApplyModifiedProperties();
			            }
		            }
	            }
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
	        return 40;
        }
    }
}
