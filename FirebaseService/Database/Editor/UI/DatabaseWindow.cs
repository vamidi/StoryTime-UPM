using System;

using UnityEditor;
using UnityEngine;

using UnityToolbarExtender;

namespace StoryTime.FirebaseService.Database.Editor
{
	using Binary;
	using Database;
	using Configurations.ScriptableObjects;

	static class ToolbarStyles
	{
		public static readonly GUIStyle CommandButtonStyle;
		public static readonly GUIStyle CommandPopupStyle;

		static ToolbarStyles()
		{
			CommandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 16,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold,
			};

			CommandPopupStyle = new GUIStyle("Command")
			{
				fontSize = 16,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold,
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

		private const string EditorPrefSelectedValueKey = "StoryTime-Toolbar-Selected-Index";

		private static readonly string[] TableNames;

		private static string iconLocation = "Packages/com.vamidicreations.storytime/Editor/Images/";
		private static DatabaseSyncModule Module;

		static DatabaseSyncButton()
		{
			Module = DatabaseSyncModule.Get;

			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

			SyncIcon = (Texture) EditorGUIUtility.Load($"{iconLocation}sync.png");
			SyncIconPro = (Texture) EditorGUIUtility.Load($"{iconLocation}sync-white.png");


			var tables = StoryTime.ResourceManagement.HelperClass.FindAssetsByType<TableSO>();

			// add sync all tables func
			TableNames = new string[tables.Length + 1];
			TableNames[0] = "All tables";

			// Tables
			for (int i = 1; i < tables.Length; i++)
			{
				TableNames[i] = tables[i].name;
			}
		}

		static void OnToolbarGUI()
		{
			EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
			GUILayout.FlexibleSpace();

			Texture content = EditorGUIUtility.isProSkin ? SyncIconPro : SyncIcon;

			EditorStyles.popup.fixedHeight = 22;
			ChoiceIndex = EditorGUILayout.Popup(new GUIContent(""), ChoiceIndex, TableNames,
				GUILayout.Width(100.0f), GUILayout.ExpandHeight(true));

			// When we press sync to get the table data from the server.
			if(GUILayout.Button(new GUIContent(content, "Sync the Database"), ToolbarStyles.CommandButtonStyle))
			{
				if (ChoiceIndex == 0)
				{
					Module.RequestTableUpdate();
				}
				else
				{
					TableSO table = TableDatabase.Get.GetTable(TableNames[ChoiceIndex]);
					if(table.ID != String.Empty) Module.RequestTableUpdate(table.ID);
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

		public static void Open(FirebaseConfigSO config)
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
			var config = Resources.Load<FirebaseConfigSO>(FileName);
			if (!config)
			{
				return new SerializedObject(CreateInstance<FirebaseConfigSO>());
			}

			return new SerializedObject(config);
		}
	}
}
