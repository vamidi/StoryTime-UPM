using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

using DatabaseSync.Binary;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Localization.Tables;
using Object = UnityEngine.Object;


namespace DatabaseSync.UI
{
	class ListViewTableRow : VisualElement
	{
		private const string EditorPrefConfigValueKey = "DatabaseSync-Window-Settings-Config";

		private readonly ListViewTables m_Parent;
		private readonly Table m_Table;

		internal string ScriptLocation
		{
			get => m_Table != null
				? EditorPrefs.GetString($"{EditorPrefConfigValueKey}{m_Table.metadata.title}", "")
				: "";
			set
			{
				if(m_Table != null)
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

		public ListViewTableRow(ListViewTables parent, Table table): this()
		{
			// Tables
			m_Parent = parent;
			m_Table = table;
			AssignTable(m_Table);

			if (ScriptLocation != String.Empty)
			{
				var scriptFile = AssetDatabase.LoadAssetAtPath<DatabaseConfig>(AssetDatabase.GUIDToAssetPath(ScriptLocation));
				if (scriptFile)
				{
					if (this.Q("row-object-field") is ObjectField field)
						field.value = scriptFile;
				}
			}
		}

		protected virtual SharedTableData.SharedTableEntry AddNewKey(LocalizationTableCollection tableCollection, string key)
		{
			Undo.RecordObject(tableCollection.SharedData, "Add new key");
			tableCollection.SharedData.AddKey(key);
			EditorUtility.SetDirty(tableCollection.SharedData);
			return tableCollection.SharedData.GetEntry(key);
		}

		void AssignTable(Table table)
		{
			// box.style.marginLeft = new StyleLength()
			if (this.Q("row-text-label") is Label lbl)
				lbl.text = table.metadata.title;

			if (this.Q("row-object-field") is ObjectField field)
			{
				field.objectType = typeof(MonoScript);
				field.RegisterValueChangedCallback((evt) => OnScriptAttached(evt, field));
			}

			// Add Btn
			if (this.Q("btn-create-asset") is Button btn)
				btn.clickable = new Clickable(() => CreateAssetTable(table));
			// m_Buttons.Add(btn);

			// Button config
			btn = this.Q("btn-refresh") as Button;
			if (btn != null)
			{
				btn.clickable = new Clickable(() => RefreshTable(table.id));
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
				if (monoScript.GetClass().BaseType != typeof(BaseTable<TableBehaviour>))
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

		// TODO make it possible to take the static class and the function to convert a row to the entry
		void CreateAssetTable(Table table)
		{
			var assetDirectory = EditorUtility.SaveFolderPanel("Create Table Collection", "Assets/", "");
			if (string.IsNullOrEmpty(assetDirectory))
				return;

			// only create the first data base on the first row
			if (table.Rows.Count > 0)
			{
				Dictionary<string, StringTableCollection> entries = new Dictionary<string, StringTableCollection>();
				foreach (var row in table.Rows)
				{
					foreach (var field in row.Value.Fields)
					{
						if(!entries.ContainsKey(field.Key.ColumnName))
							entries.Add(field.Key.ColumnName, LocalizationEditorSettings.CreateStringTableCollection(
								$"{table.metadata.title}_{field.Key.ColumnName}", assetDirectory,
								m_Parent.GetSelectedLocales()));

						var entry = entries[field.Key.ColumnName];

						// Keys are values inside the column
						/* SharedTableData.SharedTableEntry sharedTableEntry = */ AddNewKey(entry, field.Value.Data.ToString());
					}
				}

				var tableCollection = entries[table.Rows[0].Fields.First().Key.ColumnName];

				foreach (var ste in tableCollection.SharedData.Entries)
				{
					Debug.Log(ste.ToString());
				}

				foreach (var data in tableCollection.StringTables)
				{
					// var dataEntry = data.CreateTableEntry();
					// dataEntry.Value = field.Value.Data.ToString();
					// data.Add(sharedTableEntry.Id, dataEntry);

					Debug.Log(data.ToString());
				}

				foreach (var t in tableCollection.Tables)
				{
					var tbl = t.asset as StringTable;
					if (tbl)
					{
						foreach (var keyValue in tbl)
						{
							Debug.Log(keyValue.Key);
							Debug.Log(keyValue.Value);
						}
					}
				}
			}

			// Select the root asset and open the table editor window.
			// Selection.activeObject = createdCollection;
			// LocalizationTablesWindow.ShowWindow(createdCollection);
		}
	}
}
