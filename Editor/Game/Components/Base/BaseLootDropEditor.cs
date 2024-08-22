using System.Collections.Generic;

using UnityEngine;

using UnityEditor;
using UnityEditorInternal;

using StoryTime.Components;
using StoryTime.Domains.ItemManagement;
using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
using StoryTime.Domains.ItemManagement.Loot;
using MessageType = UnityEditor.MessageType;

namespace StoryTime.Editor.Game.Components
{
	public abstract class BaseLootDropEditor<T, TB, TS> : UnityEditor.Editor
		where T : BaseLootTable<TB, TS>
		where TB : DropStack<TS>
		where TS : ScriptableObject
	{
		private SerializedProperty _guaranteedList;
		private ReorderableList _reorderableGuaranteed;

		// Chance
		private SerializedProperty _chanceToGetList;
		private ReorderableList _reorderableChance;

		public override void OnInspectorGUI()
		{
			EditorUtility.SetDirty(target);
			T loot = (T) target;

			EditorGUILayout.BeginVertical("box");
			EditorGUI.indentLevel = 0;

			// Loot Name
			GUIStyle myStyle = new GUIStyle
			{
				normal =
				{
					textColor = GUI.color
				},
				alignment = TextAnchor.UpperCenter,
				fontStyle = FontStyle.Bold
			};
			// int _ti = myStyle.fontSize;

			EditorGUILayout.LabelField($"Loot Table", myStyle);

			myStyle.fontStyle = FontStyle.Italic;
			myStyle.fontSize = 20;

			EditorGUILayout.LabelField($"{loot.name}", myStyle);

			EditorGUILayout.Space(10);

			EditorGUI.BeginChangeCheck();

			serializedObject.Update();
			ValidateGuaranteedList(loot);
			_reorderableGuaranteed.DoLayoutList();
			ValidateOneItemFromList(loot);
			_reorderableChance.DoLayoutList();
			serializedObject.ApplyModifiedProperties();

			if (EditorGUI.EndChangeCheck())
			{
				// loot.GuaranteedLootTable
				AssignLootItemSerializedProperty(_reorderableGuaranteed, loot.guaranteedLootTable);

				// loot.OneItemFromList
				AssignLootItemSerializedProperty(_reorderableChance, loot.oneItemFromList);
			}

			// Nothing Weight
			loot.weightToNoDrop = EditorGUILayout.FloatField("No Drop Weight", loot.weightToNoDrop);

			EditorGUILayout.EndVertical();

			EditorGUILayout.Space();

			Rect r = EditorGUILayout.BeginVertical("box");
			myStyle.fontStyle = FontStyle.Bold;
			myStyle.fontSize = 20;

			EditorGUILayout.Space(5);
			EditorGUILayout.LabelField($"Drop Chance", myStyle);

			float totalWeight = loot.weightToNoDrop;
			float guaranteedHeight = 0;

			if (loot.oneItemFromList != null)
			{
				for (int j = 0; j < loot.oneItemFromList.Count; j++)
				{
					totalWeight += loot.oneItemFromList[j].Weight;
				}
			}

			var _oldColor = GUI.backgroundColor;

			if (0 < loot.guaranteedLootTable.Count) { guaranteedHeight += 10; }

			/* Guaranteed */
			GUI.backgroundColor = Color.green;
			for (int i = 0; i < loot.guaranteedLootTable.Count; i++)
			{
				guaranteedHeight += 25;

				var title = VerifyItem(loot.guaranteedLootTable[i].Item) ? GetLootName(loot.guaranteedLootTable[i].Item) : " --- No Drop Object --- ";

				EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i), r.width - 10, 20), 1,
					$"{title} [{loot.guaranteedLootTable[i].MinCountItem}-{loot.guaranteedLootTable[i].MaxCountItem}]   -   Guaranteed");
			}
			GUI.backgroundColor = _oldColor;

			/* Not Guaranteed */
			if (loot.oneItemFromList != null)
			{
				for (int i = 0; i < loot.oneItemFromList.Count; i++)
				{
					var title = VerifyItem(loot.oneItemFromList[i].Item) ? GetLootName(loot.oneItemFromList[i].Item) : "!!! No Drop Object Attachment !!!";
					if (loot.oneItemFromList[i].Weight / totalWeight < 0)
					{
						GUI.backgroundColor = Color.red;
						EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight, r.width - 10, 20), 1, "Error");
					}
					else
					{
						EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight, r.width - 10, 20),
							loot.oneItemFromList[i].Weight / totalWeight,
							$"{title} [{loot.oneItemFromList[i].MinCountItem}-{loot.oneItemFromList[i].MaxCountItem}]   -   {(loot.oneItemFromList[i].Weight / totalWeight * 100):f2}%");

					}
					GUI.backgroundColor = _oldColor;
				}
			}

			GUI.backgroundColor = Color.gray;
			EditorGUI.ProgressBar(
				new Rect(r.x + 5, r.y + 40 + (25 * loot.oneItemFromList.Count + 10) + guaranteedHeight, r.width - 10,
					20), loot.weightToNoDrop / totalWeight,
				$"Nothing Additional   -   {(loot.weightToNoDrop / totalWeight * 100):f2}%");
			GUI.backgroundColor = _oldColor;

			EditorGUILayout.Space(25 * loot.oneItemFromList.Count + 45 + guaranteedHeight);

			EditorGUILayout.EndVertical();
		}

		protected void OnEnable()
		{
			/* GUARANTEED */
			_guaranteedList = serializedObject.FindProperty("guaranteedLootTable");
			_reorderableGuaranteed = new ReorderableList(serializedObject, _guaranteedList, true, true, true, true);

			// Functions
			_reorderableGuaranteed.drawElementCallback += DrawGuaranteedListItems;
			_reorderableGuaranteed.drawHeaderCallback += DrawHeaderGuaranteed;

			/* Chance */
			_chanceToGetList = serializedObject.FindProperty("oneItemFromList");
			_reorderableChance = new ReorderableList(serializedObject, _chanceToGetList, true, true, true, true);

			// Functions
			_reorderableChance.drawElementCallback += DrawChanceListItems;
			_reorderableChance.drawHeaderCallback += DrawHeaderChance;
		}

		protected abstract bool VerifyItem(TS item);

		protected abstract string GetLootName(TS item);

		protected virtual void ValidateGuaranteedList(T loot)
		{
			bool _countError = false;
			bool _weightError = false;

			foreach (var guaranteed in loot.guaranteedLootTable)
			{
				if (guaranteed.MinCountItem <= 0) { _countError = true; }
				if (guaranteed.MinCountItem > guaranteed.MaxCountItem) { _countError = true; }
				if (guaranteed.Weight < 0) { _weightError = true; }
			}
			if (_countError) { EditorGUILayout.HelpBox("One of the List Items has an incorrect number of items, which will result in items not appearing when drawn", MessageType.Warning, true); }
			if (_weightError) { EditorGUILayout.HelpBox("One of the List Items has an incorrect Weight, this will cause erroneous data readings or the whole system will crash", MessageType.Error, true); }
		}

		protected virtual void ValidateOneItemFromList(T loot)
		{
			bool _countError = false;
			bool _weightError = false;

			foreach (var oneItem in loot.oneItemFromList)
			{
				if (oneItem.MinCountItem <= 0) { _countError = true; }
				if (oneItem.MinCountItem > oneItem.MaxCountItem) { _countError = true; }
				if (oneItem.Weight < 0) { _weightError = true; }
			}
			if (_countError) { EditorGUILayout.HelpBox("One of the List Items has an incorrect number of items, which will result in items not appearing when drawn", MessageType.Warning, true); }
			if (_weightError) { EditorGUILayout.HelpBox("One of the List Items has an incorrect Weight, this will cause erroneous data readings or the whole system will crash", MessageType.Error, true); }
		}

		private void DrawHeaderGuaranteed(Rect rect) { EditorGUI.LabelField(rect, "Guaranteed Loot Table"); }
		private void DrawHeaderChance(Rect rect) { EditorGUI.LabelField(rect, "Chance To Get Loot Table"); }

		private void DrawGuaranteedListItems(Rect rect, int index, bool isActive, bool isFocused)
		{
			T loot = (T)target;
			_reorderableGuaranteed.elementHeight = 82;
			DrawListItem(_reorderableGuaranteed, rect, index, isActive, isFocused);
		}

		private void DrawChanceListItems(Rect rect, int index, bool isActive, bool isFocused)
		{
			_reorderableChance.elementHeight = 82;
			DrawListItem(_reorderableChance, rect, index, isActive, isFocused);
		}

		private void DrawListItem(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
			if (element != null)
			{
				EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, GUIContent.none);
			}
		}

		private void AssignLootItemSerializedProperty(ReorderableList list, List<TB> stack)
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
					stack[index].Item = (TS) element.FindPropertyRelative("item").objectReferenceValue;
					stack[index].MinCountItem = element.FindPropertyRelative("minCountItem").intValue;
					stack[index].MaxCountItem = element.FindPropertyRelative("maxCountItem").intValue;
				}
			}
		}

		private void DrawProgressbar(List<TB> list, ref float guaranteedHeight, bool increaseHeight = false)
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
    [CustomPropertyDrawer(typeof(DropCharacterStack))]
	public class DropChangeItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
	        var _oldColor = GUI.backgroundColor;
	        EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var weightRectLabel = new Rect(position.x, position.y, 55, 18);
            var weightRect = new Rect(position.x, position.y + 20, 55, 18);

            EditorGUI.LabelField(weightRectLabel, "weight");
            if(property.FindPropertyRelative("weight").floatValue < 0) { GUI.backgroundColor = Color.red; }
            EditorGUI.PropertyField(weightRect, property.FindPropertyRelative("weight"), GUIContent.none);
            GUI.backgroundColor = _oldColor;

            var ItemRectLabel = new Rect(position.x + 60, position.y, position.width - 60, 18);
            var ItemRect = new Rect(position.x + 60, position.y + 20, position.width - 60 - 75, 18);

            EditorGUI.LabelField(ItemRectLabel, "Item");
            if(property.FindPropertyRelative("item").objectReferenceValue == null) { GUI.backgroundColor = Color.red; }
            var itemProp = property.FindPropertyRelative("item");
            EditorGUI.PropertyField(ItemRect, itemProp, GUIContent.none);
            GUI.backgroundColor = _oldColor;

            var MinMaxRectLabel = new Rect(position.x + position.width - 70, position.y, 70, 18);

            var MinRect = new Rect(position.x + position.width - 70, position.y + 20, 30, 18);
            var MinMaxRect = new Rect(position.x + position.width - 39, position.y + 20, 9, 18);
            var MaxRect = new Rect(position.x + position.width - 30, position.y + 20, 30, 18);

            if(property.FindPropertyRelative("minCountItem").intValue < 0) { GUI.backgroundColor = Color.red; }
            if (property.FindPropertyRelative("maxCountItem").intValue < property.FindPropertyRelative("minCountItem").intValue) { GUI.backgroundColor = Color.red; }

            EditorGUI.LabelField(MinMaxRectLabel, "Min  -  Max");
            EditorGUI.PropertyField(MinRect, property.FindPropertyRelative("minCountItem"), GUIContent.none);
            EditorGUI.LabelField(MinMaxRect, "-");
            EditorGUI.PropertyField(MaxRect, property.FindPropertyRelative("maxCountItem"), GUIContent.none);
            GUI.backgroundColor = _oldColor;

            /*
            if (itemProp is {objectReferenceValue: CharacterSO characterRef})
            {
	            SerializedObject serializedObject = new SerializedObject(characterRef);
	            serializedObject.ApplyModifiedProperties();
            }
            */

            // Show item label
            if (itemProp is {objectReferenceValue: ItemSO itemRef})
            {
	            SerializedObject serializedObject = new SerializedObject(itemRef);
	            var prefabProp = serializedObject.FindProperty("itemSettings.prefab");

	            using (new EditorGUI.DisabledGroupScope(prefabProp == null))
	            {
		            if (prefabProp != null)
		            {
			            EditorGUI.LabelField(new Rect(position.x, position.y + 40, 55, 18), "Item drop");
			            EditorGUI.PropertyField(new Rect(position.x + 60, position.y + 40, position.width - 60 - 75, 18), prefabProp, GUIContent.none);
			            // serializedObject.ApplyModifiedProperties();
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
