using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Reporting;
using UnityEditor.Localization.Plugins.Google.Columns;

using UnityEngine;

namespace StoryTime.Editor.Domains.Localization.Plugins.JSON
{
	using Fields;
	using StoryTime.Domains.Database;
	using StoryTime.Editor.Localization;
	using StoryTime.Domains.Utilities.Extensions;
	using StoryTime.Domains.Settings.ScriptableObjects;
	using StoryTime.Editor.Domains.Localization.UI.Utilities;
	
	class JsonExtensionPropertyDrawerData
	{
		public SerializedProperty Collection;

		public SerializedProperty JsonServiceProvider;

		public SerializedProperty Fields;

		public SerializedProperty TableName;
		public SerializedProperty TableId;

		public SerializedProperty RemoveMissingPulledKeys;

		public ReorderableListExtended FieldsList;

		public Task PushTask;

		public StoryTimeSettingsSO Provider => JsonServiceProvider.objectReferenceValue as StoryTimeSettingsSO;

		// public string m_NewJsonName;
	}

	[CustomPropertyDrawer(typeof(JsonExtension))]
	class JsonExtensionPropertyDrawer : PropertyDrawerExtended<JsonExtensionPropertyDrawerData>
	{
		class Styles
		{
			public static readonly GUIContent AddDefaultColumns = new GUIContent("Add Default fields");

			// public static readonly GUIContent addLocalesInSheet = new GUIContent("Add Project Locales Found In Sheet");
			// public static readonly GUIContent addSheet = new GUIContent("Add Sheet");
			// public static readonly GUIContent createNewSpredsheet = new GUIContent("Create New Spreadsheet");
			public static readonly GUIContent ExtractColumns = new GUIContent("Extract fields From File");

			public static readonly GUIContent Header = new GUIContent("JSON File");

			// public static readonly GUIContent newSheetName = new GUIContent("Sheet Name");
			public static readonly GUIContent NoFileFound = new GUIContent("No File Could Be Found");
			public static readonly GUIContent MappedFields = new GUIContent("Mapped Fields");

			public static readonly GUIContent OpenTable =
				new GUIContent("Open", "Opens the table in an external browser");

			public static readonly GUIContent Push = new GUIContent("Push");
			public static readonly GUIContent Pull = new GUIContent("Pull");
			public static readonly GUIContent PushSelected = new GUIContent("Push Selected");
			public static readonly GUIContent PullSelected = new GUIContent("Pull Selected");
			public static readonly GUIContent SelectFile = new GUIContent("Select File");

			// public static readonly GUIContent TableName = new GUIContent("Table Name",
				// "The Sheet Id from your Google Spreadsheet. In the Spreadsheet’s Google URL, this is at the end of the URL: https://docs.google.com/spreadsheets/d/SpreadhsheetId/edit#gid=sheetId");

			// public static readonly GUIContent TableId = new GUIContent("Table Id",
				// "The Spreadsheet Id from your Google Spreadsheet. In the Spreadsheet’s Google URL, this is in the middle of the URL: https://docs.google.com/spreadsheets/d/SpreadhsheetId/edit#gid=sheetId");
		}

		public override JsonExtensionPropertyDrawerData CreatePropertyData(SerializedProperty property)
		{
			var data = new JsonExtensionPropertyDrawerData
			{
				Collection = property.FindPropertyRelative("m_Collection"),
				TableName = property.FindPropertyRelative("tableName"),
				TableId = property.FindPropertyRelative("tableId"),
				JsonServiceProvider = property.FindPropertyRelative("jsonServiceProvider"),
				Fields = property.FindPropertyRelative("fields"),
				RemoveMissingPulledKeys = property.FindPropertyRelative("removeMissingPulledKeys")
			};

			var ltc = data.Collection.objectReferenceValue as LocalizationTableCollection;
			if (ltc != null)
			{
				// data.m_NewJsonName = ltc.TableCollectionName;
				data.FieldsList = new ReorderableListExtended(property.serializedObject, data.Fields)
				{
					Header = Styles.MappedFields,
					AddMenuType = typeof(SheetColumn),
					AddMenuItems = menu =>
					{
						menu.AddSeparator(string.Empty);
						menu.AddItem(Styles.AddDefaultColumns, false, () =>
						{
							var columns = FieldMapping.CreateDefaultMapping();

							data.Fields.ClearArray();
							foreach (var c in columns)
							{
								var colElement = data.Fields.AddArrayElement();
								colElement.managedReferenceValue = c;
							}

							data.Fields.serializedObject.ApplyModifiedProperties();
						});

						// We can not extract the column data when using an
						// if (!data.UsingApiKey)
						// {
						if (string.IsNullOrEmpty(data.TableId.stringValue))
						{
							menu.AddDisabledItem(Styles.ExtractColumns);
						}
						else
						{
							menu.AddItem(Styles.ExtractColumns, false, () =>
							{
								// var target = property.GetActualObjectForSerializedProperty<JsonExtension>(fieldInfo);
								var google = GetTableContent(data);
								var titles = google.GetColumnTitles();
								List<string> unused = new List<string>();
								var columns = FieldMapping.CreateMappingsFromColumnNames(titles, unused);

								if (unused.Count > 0)
								{
									Debug.Log($"Could not map: {string.Join(", ", unused)}");
								}

								data.Fields.ClearArray();
								foreach (var c in columns)
								{
									var colElement = data.Fields.AddArrayElement();
									colElement.managedReferenceValue = c;
								}

								data.Fields.serializedObject.ApplyModifiedProperties();
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

			EditorGUI.LabelField(position, Styles.Header, EditorStyles.boldLabel);
			position.MoveToNextLine();

			EditorGUI.PropertyField(position, data.JsonServiceProvider);
			position.MoveToNextLine();

			EditorGUI.BeginDisabledGroup(data.JsonServiceProvider.objectReferenceValue == null);
			using (new EditorGUI.DisabledGroupScope(data.JsonServiceProvider.objectReferenceValue == null))
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
			EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(data.TableId.stringValue));
			var sheetNamePos = position.SplitHorizontal();
			var buttonPos = sheetNamePos.right.SplitHorizontal();
			data.TableId.stringValue = EditorGUI.TextField(sheetNamePos.left, data.TableId.stringValue);

			if (GUI.Button(buttonPos.left, Styles.OpenTable))
			{
				JsonTableSync.OpenTableInBrowser(data.Provider, data.TableId.stringValue);
			}
			EditorGUI.EndDisabledGroup();
#endregion inner

			if (EditorGUI.DropdownButton(buttonPos.right, Styles.SelectFile, FocusType.Passive))
			{
				try
				{
					GetTableContent(data);

					var tables = TableDatabase.Get.Tables;

					var menu = new GenericMenu();
					foreach (var table in tables)
					{
						menu.AddItem(new GUIContent(table.Value.asset.Metadata.title.UcFirst()), false, () =>
						{
							data.TableName.stringValue = table.Value.asset.Metadata.title;
							data.TableId.stringValue = table.Key;
							data.TableId.serializedObject.ApplyModifiedProperties();
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
			                                        string.IsNullOrEmpty(data.TableId.stringValue) ||
			                                        data.FieldsList.count == 0))
			{
				using (new EditorGUI.DisabledGroupScope(data.FieldsList.index < 0))
				{
					if (GUI.Button(splitRow.left, Styles.PushSelected))
					{
						// var google = GetGoogleSheets(data);
						// var target = property.GetActualObjectForSerializedProperty<GoogleSheetsExtension>(fieldInfo);
						// var selectedCollection = GetSelectedColumns(data.columnsList.index, property);
						// var collection = target.TargetCollection as StringTableCollection;
						// data.pushTask = google.PushStringTableCollectionAsync(data.m_SheetId.intValue, collection, selectedCollection, TaskReporter.CreateDefaultReporter());
						// s_PushRequests.Add((collection, data.pushTask));
					}

					// TODO able to select certain rows
					if (GUI.Button(splitRow.right, Styles.PullSelected))
					{
						// var google = GetGoogleSheets(data);
						// var target = property.GetActualObjectForSerializedProperty<GoogleSheetsExtension>(fieldInfo);
						// var selectedCollection = GetSelectedColumns(data.columnsList.index, property);
						// google.PullIntoStringTableCollection(data.m_SheetId.intValue, target.TargetCollection as StringTableCollection, selectedCollection, data.m_RemoveMissingPulledKeys.boolValue, TaskReporter.CreateDefaultReporter(), true);
					}
				}

				splitRow = position.SplitHorizontal();
				position.MoveToNextLine();
				// TODO able to push data back to firebase
				if (GUI.Button(splitRow.left, Styles.Push))
				{
					// var google = GetGoogleSheets(data);
					// var target = property.GetActualObjectForSerializedProperty<GoogleSheetsExtension>(fieldInfo);
					// var collection = target.TargetCollection as StringTableCollection;
					// data.pushTask = google.PushStringTableCollectionAsync(data.m_SheetId.intValue, collection, target.Columns, TaskReporter.CreateDefaultReporter());
					// s_PushRequests.Add((collection, data.pushTask));
				}

				if (GUI.Button(splitRow.right, Styles.Pull))
				{
					if (!ValidatePull(data))
					{
						return;
					}

					var google = GetTableContent(data);
					var target = property.GetActualObjectForSerializedProperty<JsonExtension>(fieldInfo);
					google.PullIntoStringTableCollection(target.TargetCollection as StringTableCollection, target.Fields,
						data.RemoveMissingPulledKeys.boolValue, TaskReporter.CreateDefaultReporter(), true);
				}
			}
		}

		JsonTableSync GetTableContent(JsonExtensionPropertyDrawerData data)
		{
			var google = new JsonTableSync
			{
				TableId = data.TableId.stringValue
			};
			return google;
		}

		private bool ValidatePull(JsonExtensionPropertyDrawerData data)
		{
			StringTableCollection createdCollection = LocalizationEditorSettings.GetStringTableCollection(data.TableName.stringValue.UcFirst());

			return createdCollection != null && EditorUtility.DisplayDialog("Existing data found!",
				"Are you sure you want to replace the existing data?", "Overwrite", "Cancel");
		}
	}
}
