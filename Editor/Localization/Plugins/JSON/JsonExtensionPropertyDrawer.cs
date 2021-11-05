using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Reporting;
using UnityEditor.Localization.Plugins.Google.Columns;

using StoryTime.Localization;

namespace StoryTime.Editor.Localization.Plugins.JSON
{
	using UI;
	using Fields;
	using ResourceManagement.Util;
	using Configurations.ScriptableObjects;

	class JsonExtensionPropertyDrawerData
	{
		public SerializedProperty m_Collection;

		public SerializedProperty m_JsonServiceProvider;

		public SerializedProperty m_Fields;

		public SerializedProperty m_TableId;

		public SerializedProperty m_RemoveMissingPulledKeys;

		public ReorderableListExtended FieldsList;

		public Task PushTask;

		public DatabaseConfigSO Provider => m_JsonServiceProvider.objectReferenceValue as DatabaseConfigSO;

		// public string m_NewJsonName;
	}

	[CustomPropertyDrawer(typeof(JsonExtension))]
	class JsonExtensionPropertyDrawer : PropertyDrawerExtended<JsonExtensionPropertyDrawerData>
	{
		class Styles
		{
			public static readonly GUIContent addDefaultColumns = new GUIContent("Add Default fields");

			// public static readonly GUIContent addLocalesInSheet = new GUIContent("Add Project Locales Found In Sheet");
			// public static readonly GUIContent addSheet = new GUIContent("Add Sheet");
			// public static readonly GUIContent createNewSpredsheet = new GUIContent("Create New Spreadsheet");
			public static readonly GUIContent extractColumns = new GUIContent("Extract fields From File");

			public static readonly GUIContent header = new GUIContent("JSON File");

			// public static readonly GUIContent newSheetName = new GUIContent("Sheet Name");
			public static readonly GUIContent NoFileFound = new GUIContent("No File Could Be Found");
			public static readonly GUIContent MappedFields = new GUIContent("Mapped Fields");

			public static readonly GUIContent openTable =
				new GUIContent("Open", "Opens the table in an external browser");

			public static readonly GUIContent push = new GUIContent("Push");
			public static readonly GUIContent pull = new GUIContent("Pull");
			public static readonly GUIContent pushSelected = new GUIContent("Push Selected");
			public static readonly GUIContent pullSelected = new GUIContent("Pull Selected");
			public static readonly GUIContent SelectFile = new GUIContent("Select File");

			public static readonly GUIContent TableName = new GUIContent("Table Name",
				"The Sheet Id from your Google Spreadsheet. In the Spreadsheet’s Google URL, this is at the end of the URL: https://docs.google.com/spreadsheets/d/SpreadhsheetId/edit#gid=sheetId");

			public static readonly GUIContent TableId = new GUIContent("Table Id",
				"The Spreadsheet Id from your Google Spreadsheet. In the Spreadsheet’s Google URL, this is in the middle of the URL: https://docs.google.com/spreadsheets/d/SpreadhsheetId/edit#gid=sheetId");
		}

		public override JsonExtensionPropertyDrawerData CreatePropertyData(SerializedProperty property)
		{
			var data = new JsonExtensionPropertyDrawerData
			{
				m_Collection = property.FindPropertyRelative("m_Collection"),
				m_JsonServiceProvider = property.FindPropertyRelative("jsonServiceProvider"),
				m_TableId = property.FindPropertyRelative("tableId"),
				m_Fields = property.FindPropertyRelative("fields"),
				m_RemoveMissingPulledKeys = property.FindPropertyRelative("removeMissingPulledKeys")
			};

			var ltc = data.m_Collection.objectReferenceValue as LocalizationTableCollection;
			if (ltc != null)
			{
				// data.m_NewJsonName = ltc.TableCollectionName;
				data.FieldsList = new ReorderableListExtended(property.serializedObject, data.m_Fields)
				{
					Header = Styles.MappedFields,
					AddMenuType = typeof(SheetColumn),
					AddMenuItems = menu =>
					{
						menu.AddSeparator(string.Empty);
						menu.AddItem(Styles.addDefaultColumns, false, () =>
						{
							var columns = FieldMapping.CreateDefaultMapping();

							data.m_Fields.ClearArray();
							foreach (var c in columns)
							{
								var colElement = data.m_Fields.AddArrayElement();
								colElement.managedReferenceValue = c;
							}

							data.m_Fields.serializedObject.ApplyModifiedProperties();
						});

						// We can not extract the column data when using an
						// if (!data.UsingApiKey)
						// {
						if (string.IsNullOrEmpty(data.m_TableId.stringValue))
						{
							menu.AddDisabledItem(Styles.extractColumns);
						}
						else
						{
							menu.AddItem(Styles.extractColumns, false, () =>
							{
								var target = property.GetActualObjectForSerializedProperty<JsonExtension>(fieldInfo);
								var google = GetTableContent(data);
								var titles = google.GetColumnTitles();
								List<string> unused = new List<string>();
								var columns = FieldMapping.CreateMappingsFromColumnNames(titles, unused);

								if (unused.Count > 0)
								{
									Debug.Log($"Could not map: {string.Join(", ", unused)}");
								}

								data.m_Fields.ClearArray();
								foreach (var c in columns)
								{
									var colElement = data.m_Fields.AddArrayElement();
									colElement.managedReferenceValue = c;
								}

								data.m_Fields.serializedObject.ApplyModifiedProperties();
							});
						}

						// }
					}
				};
			}

			// $"{ TableBinary.Fetch().dataPath }/dialogues.json"

			return data;
		}

		public override void OnGUI(JsonExtensionPropertyDrawerData data, Rect position, SerializedProperty property,
			GUIContent label)
		{
			position.yMin += EditorGUIUtility.standardVerticalSpacing;
			position.height = EditorGUIUtility.singleLineHeight;

			EditorGUI.LabelField(position, Styles.header, EditorStyles.boldLabel);
			position.MoveToNextLine();

			EditorGUI.PropertyField(position, data.m_JsonServiceProvider);
			position.MoveToNextLine();

			EditorGUI.BeginDisabledGroup(data.m_JsonServiceProvider.objectReferenceValue == null);
			using (new EditorGUI.DisabledGroupScope(data.m_JsonServiceProvider.objectReferenceValue == null))
			{
				DrawTableNameField(data, ref position);
				DrawColumnsField(data, ref position);
				DrawSyncControls(data, property, ref position);
			}
		}

		public override float GetPropertyHeight(JsonExtensionPropertyDrawerData data, SerializedProperty property,
			GUIContent label)
		{
			float height = EditorGUIUtility.standardVerticalSpacing; // top padding
			height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 10;
			height += data.FieldsList.GetHeight() + EditorGUIUtility.standardVerticalSpacing;
			return height;
		}

		void DrawTableNameField(JsonExtensionPropertyDrawerData data, ref Rect position)
		{
#region inner
			EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(data.m_TableId.stringValue));
			var sheetNamePos = position.SplitHorizontal();
			var buttonPos = sheetNamePos.right.SplitHorizontal();
			data.m_TableId.stringValue = EditorGUI.TextField(sheetNamePos.left, data.m_TableId.stringValue);

			if (GUI.Button(buttonPos.left, Styles.openTable))
			{
				JsonTableSync.OpenTableInBrowser(data.m_TableId.stringValue);
			}
			EditorGUI.EndDisabledGroup();
#endregion inner

			if (EditorGUI.DropdownButton(buttonPos.right, Styles.SelectFile, FocusType.Passive))
			{
				try
				{
					var google = GetTableContent(data);
					var files = HelperClass.GetDataFiles();

					var menu = new GenericMenu();
					foreach (var s in files)
					{
						menu.AddItem(new GUIContent(s.name), false, () =>
						{
							data.m_TableId.stringValue = s.fileName;
							data.m_TableId.serializedObject.ApplyModifiedProperties();
						});
					}

					if (menu.GetItemCount() == 0)
						menu.AddDisabledItem(Styles.NoFileFound);

					menu.DropDown(sheetNamePos.right);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}
			}

			position.MoveToNextLine();
		}

		void DrawColumnsField(JsonExtensionPropertyDrawerData data, ref Rect position)
		{
			position.height = data.FieldsList.GetHeight();
			data.FieldsList.DoList(position);
			position.MoveToNextLine();
			position.height = EditorGUIUtility.singleLineHeight;
			position.MoveToNextLine();
		}

		void DrawSyncControls(JsonExtensionPropertyDrawerData data, SerializedProperty property, ref Rect position)
		{
			// EditorGUI.PropertyField(position, data.m_RemoveMissingPulledKeys);
			// position.MoveToNextLine();

			// Disable if we have no destination sheet.
			var splitRow = position.SplitHorizontal();
			position.MoveToNextLine();

			if (data.PushTask != null && data.PushTask.IsCompleted)
			{
				// var target = property.GetActualObjectForSerializedProperty<GoogleSheetsExtension>(fieldInfo);
				// var collection = target.TargetCollection as StringTableCollection;
				// var currentPushRequest = s_PushRequests.FirstOrDefault(tc => ReferenceEquals(tc.collection, collection));
				// s_PushRequests.Remove(currentPushRequest);
				data.PushTask = null;
			}

			using (new EditorGUI.DisabledGroupScope(data.PushTask != null ||
			                                        string.IsNullOrEmpty(data.m_TableId.stringValue) ||
			                                        data.FieldsList.count == 0))
			{
				using (new EditorGUI.DisabledGroupScope(data.FieldsList.index < 0))
				{
					if (GUI.Button(splitRow.left, Styles.pushSelected))
					{
						// var google = GetGoogleSheets(data);
						// var target = property.GetActualObjectForSerializedProperty<GoogleSheetsExtension>(fieldInfo);
						// var selectedCollection = GetSelectedColumns(data.columnsList.index, property);
						// var collection = target.TargetCollection as StringTableCollection;
						// data.pushTask = google.PushStringTableCollectionAsync(data.m_SheetId.intValue, collection, selectedCollection, TaskReporter.CreateDefaultReporter());
						// s_PushRequests.Add((collection, data.pushTask));
					}

					if (GUI.Button(splitRow.right, Styles.pullSelected))
					{
						// var google = GetGoogleSheets(data);
						// var target = property.GetActualObjectForSerializedProperty<GoogleSheetsExtension>(fieldInfo);
						// var selectedCollection = GetSelectedColumns(data.columnsList.index, property);
						// google.PullIntoStringTableCollection(data.m_SheetId.intValue, target.TargetCollection as StringTableCollection, selectedCollection, data.m_RemoveMissingPulledKeys.boolValue, TaskReporter.CreateDefaultReporter(), true);
					}
				}

				splitRow = position.SplitHorizontal();
				position.MoveToNextLine();
				if (GUI.Button(splitRow.left, Styles.push))
				{
					// var google = GetGoogleSheets(data);
					// var target = property.GetActualObjectForSerializedProperty<GoogleSheetsExtension>(fieldInfo);
					// var collection = target.TargetCollection as StringTableCollection;
					// data.pushTask = google.PushStringTableCollectionAsync(data.m_SheetId.intValue, collection, target.Columns, TaskReporter.CreateDefaultReporter());
					// s_PushRequests.Add((collection, data.pushTask));
				}

				if (GUI.Button(splitRow.right, Styles.pull))
				{
					var google = GetTableContent(data);
					var target = property.GetActualObjectForSerializedProperty<JsonExtension>(fieldInfo);
					google.PullIntoStringTableCollection(target.TargetCollection as StringTableCollection, target.Fields,
						data.m_RemoveMissingPulledKeys.boolValue, TaskReporter.CreateDefaultReporter(), true);
				}
			}
		}

		JsonTableSync GetTableContent(JsonExtensionPropertyDrawerData data)
		{
			var google = new JsonTableSync(data.Provider)
			{
				TableId = data.m_TableId.stringValue
			};
			return google;
		}
	}
}
