using System;

using UnityEditor;

using UnityEngine;

using UnityToolbarExtender;

namespace DatabaseSync.Editor
{
	using Binary;
	using Database;
	using ResourceManagement.Util;
	static class ToolbarStyles
	{
		public static readonly GUIStyle CommandButtonStyle;

		static ToolbarStyles()
		{
			CommandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 16,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}

	[InitializeOnLoad]
	public class DatabaseSyncButton
	{
		public static readonly Texture SyncIcon;
		public static readonly Texture SyncIconPro;

		private static int ChoiceIndex
		{
			get => EditorPrefs.GetInt(EditorPrefSelectedValueKey, 0);
			set => EditorPrefs.SetInt(EditorPrefSelectedValueKey, value);
		}

		private const string EditorPrefSelectedValueKey = "DatabaseSync-Toolbar-Selected-Index";

		private static readonly string[] TableNames;

		private static string iconLocation = "Packages/com.unity.vamidicreations.storytime/Editor/Images/";

		static DatabaseSyncButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

			SyncIcon = (Texture) EditorGUIUtility.Load($"{iconLocation}sync.png");
			SyncIconPro = (Texture) EditorGUIUtility.Load($"{iconLocation}sync-white.png");

			var tables = HelperClass.GetDataFiles();
			// add sync all tables func
			TableNames = new string[tables.Count + 1];
			TableNames[0] = "All tables";

			// Tables
			for (int i = 1; i < tables.Count; i++)
			{
				TableNames[i] = tables[i].name;
			}
		}

		static void OnToolbarGUI()
		{
			EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
			GUILayout.FlexibleSpace();

			Texture content = EditorGUIUtility.isProSkin ? SyncIconPro : SyncIcon;

			ChoiceIndex = EditorGUILayout.Popup(new GUIContent(""), ChoiceIndex, TableNames,
				GUILayout.Width(100.0f), GUILayout.ExpandHeight(true));

			// When we press sync to get the table data from the server.
			if(GUILayout.Button(new GUIContent(content, "Sync the Database"), ToolbarStyles.CommandButtonStyle))
			{
				if (ChoiceIndex == 0)
				{
					DatabaseSyncModule.Get.RequestTableUpdate();
				}
				else
				{
					Table table = TableDatabase.Get.GetTable(TableNames[ChoiceIndex]);
					if(table.id != String.Empty) DatabaseSyncModule.Get.RequestTableUpdate(table.id);
				}
			}

			EditorGUI.EndDisabledGroup();
		}
	}

	public class DatabaseWindow : EditorWindow
	{
		public enum PropertyFlags
		{
			Normal = (1 << 0),
			Password = (1 << 1),
		}

		private static SerializedObject s_DatabaseConfig;
		public static readonly string FileName = "DatabaseConfig";

		public static void Open(DatabaseConfig config)
		{
			s_DatabaseConfig = new SerializedObject(config);
		}

		public void OnEnable()
		{
			s_DatabaseConfig = LoadConfig();
		}

		public void OnGUI()
		{
			// window
			EditorGUILayout.LabelField("Database Configurations");

			EditorGUILayout.Space(10, true);

			DrawProperty(s_DatabaseConfig.FindProperty("email"), false);
			DrawProperty(s_DatabaseConfig.FindProperty("password"), false, PropertyFlags.Password);

			EditorGUILayout.Space(10, true);

			if (GUILayout.Button("Save Presets", EditorStyles.boldLabel))
			{
				SaveConfig();
			}
		}

		private void DrawProperty(SerializedProperty prop, bool drawChildren, PropertyFlags flags = PropertyFlags.Normal)
		{
			if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic)
			{
				EditorGUILayout.BeginHorizontal();
				prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName);
				EditorGUILayout.EndHorizontal();

				if (prop.isExpanded)
				{
					EditorGUI.indentLevel++;
					DrawProperty(prop, drawChildren);
					EditorGUI.indentLevel--;
				}
			}
			else
			{
				switch (flags)
				{
					case PropertyFlags.Normal:
					default:
						EditorGUILayout.PropertyField(prop, drawChildren);
						break;
					case PropertyFlags.Password:
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Password");
						EditorGUILayout.PasswordField(prop.stringValue);
						EditorGUILayout.EndHorizontal();
						break;
				}

			}
		}

		private void SaveConfig()
		{
			/*
			if (_databaseConfig == null)
				return;

			var configFile = CreateInstance<DatabaseConfig>();

			configFile.Email = _databaseConfig.FindProperty("email").stringValue;
			configFile.password = _databaseConfig.FindProperty("password").stringValue;

			// If folder does not exist make one.
			if (!AssetDatabase.IsValidFolder("Assets/Resources"))
				AssetDatabase.CreateFolder("Assets", "Resources");

			if (AssetDatabase.Contains(configFile)) { AssetDatabase.DeleteAsset($"Assets/Resources/{FileName}.asset"); }

			AssetDatabase.CreateAsset(configFile, $"Assets/Resources/{FileName}.asset");
			AssetDatabase.SaveAssets();
			*/
		}

		private SerializedObject LoadConfig()
		{
			var config = Resources.Load<DatabaseConfig>(FileName);
			if (!config)
			{
				return new SerializedObject(CreateInstance<DatabaseConfig>());
			}

			return new SerializedObject(config);
		}
	}
}
