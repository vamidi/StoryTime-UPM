using System;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StoryTime.FirebaseService.Database.Editor.UI
{
	using Binary;
	using Database;
	using StoryTime.Editor.UI;

	class ListViewTableRow : VisualElement
	{
		private const string EditorPrefConfigValueKey = "StoryTime-Window-Settings-Config";

		private readonly string m_TableName;

		internal string ScriptLocation
		{
			get => m_TableName != null
				? EditorPrefs.GetString($"{EditorPrefConfigValueKey}{m_TableName}", "")
				: "";
			set
			{
				if(m_TableName != null)
					EditorPrefs.SetString(EditorPrefConfigValueKey, value);
			}
		}

		internal new class UxmlFactory : UxmlFactory<ListViewTableRow> { }

		public ListViewTableRow()
		{
			var styles = Resources.GetStyleAsset(nameof(ListViewTables));
			styleSheets.Add(styles);

			var asset = Resources.GetTemplateAsset(nameof(ListViewTableRow));
			asset.CloneTree(this);
		}

		public ListViewTableRow(string title): this()
		{
			// Tables
			m_TableName = title;
			AssignTable();

			if (ScriptLocation != String.Empty)
			{
				var scriptFile = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(ScriptLocation));
				if (scriptFile)
				{
					if (this.Q("row-object-field") is ObjectField field)
						field.value = scriptFile;
				}
			}
		}

/*
		protected virtual SharedTableData.SharedTableEntry AddNewKey(LocalizationTableCollection tableCollection, string key)
		{
			Undo.RecordObject(tableCollection.SharedData, "Add new key");
			tableCollection.SharedData.AddKey(key);
			EditorUtility.SetDirty(tableCollection.SharedData);
			return tableCollection.SharedData.GetEntry(key);
		}
*/
		void AssignTable()
		{
			// box.style.marginLeft = new StyleLength()
			if (this.Q("row-text-label") is Label lbl)
				lbl.text = m_TableName;

			if (this.Q("row-object-field") is ObjectField field)
			{
				field.objectType = typeof(MonoScript);
				field.RegisterValueChangedCallback((evt) => OnScriptAttached(evt, field));
			}

			// Add Btn
			// if (this.Q("btn-create-asset") is Button btn)
				// btn.clickable = new Clickable(() => CreateAssetTable(table));
			// m_Buttons.Add(btn);

			// Button config
			if (this.Q("btn-refresh") is Button btn)
			{
				btn.clickable = new Clickable(() =>
				{
					TableSO table = TableDatabase.Get.GetTable(m_TableName);
					if(table.ID != String.Empty) RefreshTable(table.ID);
				});
			}
		}

		void RefreshTable(string tableID)
		{
			DatabaseSyncModule.Get.RequestTableUpdate(tableID);
		}

		void OnScriptAttached(ChangeEvent<Object> evt, ObjectField field)
		{
			var monoScript = evt.newValue as MonoScript;
			if (monoScript != null)
			{
				if (monoScript.GetClass().BaseType != typeof(BaseTable<StoryTime.Components.ScriptableObjects.TableBehaviour>))
				{
					field.SetValueWithoutNotify(evt.previousValue);
					Debug.Log("Class does not inherit from BaseTable");
				}

				ScriptLocation = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(monoScript));

				object[] rows = {new TableRow()};
				var method = monoScript.GetClass().GetMethod("ConvertRow");
				if (method != null)
					method.Invoke(null, rows); // (null, null) means calling static method with no parameters
			}
		}
	}
}
