using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

using UnityEngine;
using UnityEngine.UIElements;

using TMPro;
using UnityEditor.Localization;
using UnityEditor.Localization.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

using StoryTime.Database;
using StoryTime.Utils.Extensions;
using StoryTime.Utils.Configurations;
using StoryTime.Database.ScriptableObjects;
using UnityEditor.Localization.Reporting;

// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.UI
{
	using Localization.Plugins.JSON;
	using Localization.Plugins.JSON.Fields;


	// using FirebaseService.Database;
	using FirebaseService.Settings;
	using Configurations.ScriptableObjects;

	public class StoryTimeSettingsProvider : SettingsProvider
	{
		private SerializedObject settings;
		private SerializedObject globalSettings;
		private SerializedObject dialogueSettings;

		private const string defaultDialogueTable = "dialogues";

		// Register the SettingsProvider
		[SettingsProvider]
		public static SettingsProvider CreateMyCustomSettingsProvider()
		{
			// First parameter is the path in the Settings window.
			// Second parameter is the scope of this setting: it only appears in the Settings window for the Project scope.
			var provider = new StoryTimeSettingsProvider();

			// Automatically extract all keywords from the Styles.
			// provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
			return provider;
		}

		StoryTimeSettingsProvider() : base("Project/MyCustomUIElementsSettings", SettingsScope.Project)
		{
			label = "StoryTime";

			// activateHandler is called when the user clicks on the Settings item in the Settings window.
			// activateHandler = (_, rootElement) =>
			// {
			//
			// };
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			Debug.Log("Activate");

			// This function is called when the user clicks on the MyCustom element in the Settings window.
			settings = FirebaseConfigSO.GetSerializedSettings();
			globalSettings = GlobalSettingsSO.GetSerializedSettings();

			var dialogueConfig = ((GlobalSettingsSO)globalSettings.targetObject).DialogueSettings;
			if(dialogueConfig) {
				dialogueSettings = new SerializedObject(dialogueConfig);
			}

			var asset = Resources.GetTemplateAsset("DatabaseSyncWindow");
			var styles = Resources.GetStyleAsset("DatabaseSyncWindow");

			asset.CloneTree(rootElement);
			rootElement.styleSheets.Add(styles);
			Initialize(rootElement);

			/*
			// rootElement is a VisualElement. If you add any children to it, the OnGUI function
			// isn't called because the SettingsProvider uses the UIElements drawing framework.
			var title = new Label() {
				text = "StoryTime Settings"
			};
			title.AddToClassList("title");
			rootElement.Add(title);

			var properties = new VisualElement() {
				style =
				{
					flexDirection = FlexDirection.Column
				}
			};
			properties.AddToClassList("property-list");
			rootElement.Add(properties);

			properties.Add(new InspectorElement(settings));

			rootElement.Bind(settings);
			*/
		}

		// TODO refactor this because this is duplicate code from DatabaseSyncWindow
		private void Initialize(VisualElement root)
		{
			// Grab the root settings elements
			var globalSettingsField = root.Q<VisualElement>("globalSettings");

			// First get the settings field and bind it to the config file.
			var globalConfigField = root.Q<ObjectField>("global-config-field");
			globalConfigField.objectType = typeof(GlobalSettingsSO);
			globalConfigField.value = globalSettings.targetObject;
			globalConfigField.Bind(globalSettings);

			globalConfigField.RegisterValueChangedCallback((evt) =>
			{
				var config = evt.newValue as GlobalSettingsSO;
				if (config != null)
				{
					// then get the config file is selected
					globalConfigField.Bind(new SerializedObject(config));
					globalSettingsField.style.display = DisplayStyle.Flex;
					// FirebaseSyncConfig.SelectedConfig = AssetDatabase.GetAssetPath(config);

					// Save the current location
					PathLocations.SavePathLocations(new PathLocations
					{
						GlobalSettings = ResourceManagement.HelperClass.GetAssetPath(config),
						FirebaseSettings = ResourceManagement.HelperClass.GetAssetPath(settings.targetObject),
						DialogueSettings = ResourceManagement.HelperClass.GetAssetPath(dialogueSettings.targetObject),
					});
				}
				else
				{
					globalConfigField.Unbind();
					globalSettingsField.style.display = DisplayStyle.None;
					// FirebaseSyncConfig.SelectedConfig = "";
				}
			});

			// Grab the root settings elements
			var firebaseSettingsField = root.Q<VisualElement>("firebaseSettings");

			// First get the settings field and bind it to the config file.
			var field = root.Q<ObjectField>("config-field");
			field.objectType = typeof(FirebaseConfigSO);
			field.value = settings.targetObject;
			field.Bind(settings);

			field.RegisterValueChangedCallback((evt) =>
			{
				var config = evt.newValue as FirebaseConfigSO;
				if (config != null)
				{
					// then get the config file is selected
					field.Bind(new SerializedObject(config));
					firebaseSettingsField.style.display = DisplayStyle.Flex;
					// FirebaseSyncConfig.SelectedConfig = AssetDatabase.GetAssetPath(config);

					PathLocations.SavePathLocations(new PathLocations
					{
						GlobalSettings = ResourceManagement.HelperClass.GetAssetPath(globalSettings.targetObject),
						FirebaseSettings = ResourceManagement.HelperClass.GetAssetPath(config),
						DialogueSettings = ResourceManagement.HelperClass.GetAssetPath(dialogueSettings.targetObject),
					});
				}
				else
				{
					field.Unbind();
					// FirebaseSyncConfig.SelectedConfig = "";
				}
			});

			// First get the dialogue config instance id if existing
			var dialogueConfigField = root.Q<ObjectField>("dialogue-config-field");
			dialogueConfigField.objectType = typeof(DialogueSettingConfigSO);
			dialogueConfigField.value = dialogueSettings.targetObject;
			dialogueConfigField.Bind(dialogueSettings);

			dialogueConfigField.RegisterValueChangedCallback((evt) =>
			{
				var config = evt.newValue as DialogueSettingConfigSO;
				if (config != null)
				{
					// then get the config file is selected
					dialogueConfigField.Bind(new SerializedObject(config));
					dialogueConfigField.style.display = DisplayStyle.Flex;
					// FirebaseSyncConfig.SelectedDialogueConfig = AssetDatabase.GetAssetPath(config);

					PathLocations.SavePathLocations(new PathLocations
					{
						GlobalSettings = ResourceManagement.HelperClass.GetAssetPath(globalSettings.targetObject),
						FirebaseSettings = ResourceManagement.HelperClass.GetAssetPath(settings.targetObject),
						DialogueSettings = ResourceManagement.HelperClass.GetAssetPath(config),
					});
				}
				else
				{
					dialogueConfigField.Unbind();
					// FirebaseSyncConfig.SelectedDialogueConfig = "";
				}
			});

			PopulateProperties(root);

	        // TODO able to reset the token if users are switching to different environments.

	        /*
	        m_TabToggles = root.Query<ToolbarToggle>().ToList();
            m_TabPanels = new List<VisualElement>();
            for (int i = 0; i < m_TabToggles.Count; ++i)
            {
                var toggle = m_TabToggles[i];
                var panelName = $"{toggle.name}-panel";
                var panel = root.Q(panelName);
                Debug.Assert(panel != null, $"Could not find panel \"{panelName}\"");
                m_TabPanels.Add(panel);
                panel.style.display = SelectedTab == i ? DisplayStyle.Flex : DisplayStyle.None;
                toggle.value = SelectedTab == i;
                int idx = i;
                toggle.RegisterValueChangedCallback((chg) => TabSelected(idx));
            }
            Debug.Assert(m_TabPanels.Count == m_TabToggles.Count, "Expected the same number of tab toggle buttons and panels.");
            */
		}

		private void SaveConfig(FirebaseConfigSO configFile, DialogueSettingConfigSO dialogueSettingConfig = null)
		{
			if (configFile != null)
			{
				Debug.Log("Saving Database Config file");
				EditorUtility.SetDirty(configFile);
			}

			if (dialogueSettingConfig != null)
			{
				EditorUtility.SetDirty(dialogueSettingConfig);
			}

			AssetDatabase.SaveAssets();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="root"></param>
		private void PopulateProperties(VisualElement root)
		{
			// Global settings

			SerializedProperty dataPathProp = globalSettings.FindProperty("dataPath");
			root.Q<TextField>("data-path-field").BindProperty(dataPathProp);

			root.Q<Button>("btn-choose-path").clickable.clicked += () =>
			{
				var assetDirectory = EditorUtility.SaveFolderPanel("Data location", Application.dataPath, "");
				if (string.IsNullOrEmpty(assetDirectory))
					return;

				dataPathProp.stringValue = assetDirectory;
				globalSettings.ApplyModifiedProperties();
				// SaveConfig(configFile);
			};

			// configure the buttons
			root.Q<Button>("btn-save").clickable.clicked += () => SaveConfig(settings.targetObject as FirebaseConfigSO, dialogueSettings.targetObject as DialogueSettingConfigSO);

			// Dialogue Settings
			SerializedProperty fontProp = dialogueSettings.FindProperty("font");
			SerializedProperty autoResizeProp = dialogueSettings.FindProperty("autoResize");
			SerializedProperty animatedTextProp = dialogueSettings.FindProperty("animatedText");
			SerializedProperty showDialogueAtOnceProp = dialogueSettings.FindProperty("showDialogueAtOnce");
			SerializedProperty voiceClipProp = dialogueSettings.FindProperty("voiceClip");
			SerializedProperty punctuationClipProp = dialogueSettings.FindProperty("punctuationClip");

			var toggle = root.Q<Toggle>("dialogue-resize-field");
			toggle.value = autoResizeProp.boolValue;
			toggle.RegisterValueChangedCallback(evt => autoResizeProp.boolValue = evt.newValue);

			toggle = root.Q<Toggle>("dialogue-animated-field");
			toggle.value = animatedTextProp.boolValue;
			toggle.RegisterValueChangedCallback(evt =>
			{
				animatedTextProp.boolValue = evt.newValue;
				dialogueSettings.ApplyModifiedProperties();
			});

			toggle = root.Q<Toggle>("dialogue-show-field");
			toggle.value = showDialogueAtOnceProp.boolValue;
			toggle.RegisterValueChangedCallback(evt =>
			{
				showDialogueAtOnceProp.boolValue = evt.newValue;
				dialogueSettings.ApplyModifiedProperties();
			});

			var audioClipField = root.Q<ObjectField>("dialogue-char-clip-field");
			audioClipField.objectType = typeof(AudioClip);
			audioClipField.value = voiceClipProp.objectReferenceValue;
			audioClipField.RegisterValueChangedCallback(evt =>
			{
				voiceClipProp.objectReferenceValue = evt.newValue as AudioClip;
				dialogueSettings.ApplyModifiedProperties();
			});

			audioClipField = root.Q<ObjectField>("dialogue-punctuation-field");
			audioClipField.objectType = typeof(AudioClip);
			audioClipField.value = punctuationClipProp.objectReferenceValue;
			audioClipField.RegisterValueChangedCallback(evt =>
			{
				punctuationClipProp.objectReferenceValue = evt.newValue as AudioClip;
				dialogueSettings.ApplyModifiedProperties();
			});

			var fontField = root.Q<ObjectField>("dialogue-font-field");
			fontField.objectType = typeof(TMP_FontAsset);
			fontField.value = fontProp.objectReferenceValue;
			fontField.RegisterValueChangedCallback((evt) =>
			{
				var newValue = evt.newValue as TMP_FontAsset;
				if (newValue != null)
				{
					fontProp.objectReferenceValue = newValue;
					dialogueSettings.ApplyModifiedProperties();
				}
			});

			// Firebase settings

			SerializedProperty databaseUrlProp = settings.FindProperty("databaseURL");
			SerializedProperty storageBucketProp = settings.FindProperty("storageBucket");
			SerializedProperty projectIDProp = settings.FindProperty("projectID");
			// SerializedProperty useServerProp = settings.FindProperty("useServer");

			root.Q<TextField>("database-url-field").BindProperty(databaseUrlProp);
			root.Q<TextField>("storage-bucket-field").BindProperty(storageBucketProp);

			var dropdown = root.Q<DropdownField>("project-id-field");
			var config = settings.targetObject as FirebaseConfigSO;
			if (config != null)
			{
				dropdown.value = config.Projects[projectIDProp.stringValue];
				dropdown.choices = config.Projects.Values.ToList();
				dropdown.RegisterValueChangedCallback(evt =>
				{
					var selected = config.Projects.FirstOrDefault(x => x.Value == evt.newValue).Key;
					projectIDProp.stringValue = selected;
					settings.ApplyModifiedProperties();
				});
			}

			// Dialogue settings
			SerializedProperty tableIdProp = dialogueSettings.FindProperty("dialogueTableId");

			string dialogueTableId = !tableIdProp.stringValue.IsNullOrEmpty()
				? tableIdProp.stringValue
				: defaultDialogueTable;

			TableSO tableSo = TableDatabase.Get.GetTable(dialogueTableId);
			dropdown = root.Q<DropdownField>("dialogue-fetch-field");
			dropdown.value = tableSo ? tableSo.Metadata.title.UcFirst() : "";
			dropdown.choices = TableDatabase.Get.GetTableNames().Select(t => t.UcFirst()).ToList();
			dropdown.RegisterValueChangedCallback(evt =>
			{
				var toLower = evt.newValue.LcFirst();
				TableSO table = TableDatabase.Get.GetTableByName(toLower);
				if (!table)
				{
					return;
				}
				tableIdProp.stringValue = table.ID;
				dialogueSettings.ApplyModifiedProperties();
			});

			// TODO make it possible to import stories, items etc
			// TODO make this async if it takes a long time
			var fetchDialogueBtn = root.Q<Button>("btn-fetch-dialogue");
			fetchDialogueBtn.clickable.clicked += () =>
			{
				if (!Directory.Exists($"{Application.dataPath}/Localization"))
				{
					Debug.Log("Creating localization folder");
					Directory.CreateDirectory($"{Application.dataPath}/Localization");
				}

				ReadOnlyCollection<Locale> locales = LocalizationEditorSettings.GetLocales();
				StringTableCollection createdCollection = LocalizationEditorSettings.GetStringTableCollection(tableSo.Metadata.title.UcFirst());
				if (createdCollection == null)
				{
					createdCollection = LocalizationEditorSettings.CreateStringTableCollection(tableSo.Metadata.title.UcFirst(),
						$"{Application.dataPath}/Localization", locales);
				}

				var currentExtensions = createdCollection.Extensions;

				var jsonExtension = currentExtensions.First(c => c.GetType() == typeof(JsonExtension)) as JsonExtension;
				if (currentExtensions.IsNullOrEmpty() || jsonExtension == null)
				{
					jsonExtension = Activator.CreateInstance(typeof(JsonExtension)) as JsonExtension;

					if (jsonExtension == null)
					{
						throw new ArgumentNullException(nameof(jsonExtension));
					}

					jsonExtension.TableName = tableSo.Metadata.title;
					jsonExtension.TableId = tableSo.ID;

					var columns = FieldMapping.CreateDefaultMapping();
					jsonExtension.Fields = columns.AsReadOnly();

					createdCollection.AddExtension(jsonExtension);

					Debug.Log("Fetching dialogue data");
					LoadDialogueData(jsonExtension);
					ShowDialogueData(createdCollection);
					return;
				}

				if (EditorUtility.DisplayDialog("Dialogue data found!",
					    "Are you sure you want to replace the existing data?", "Overwrite", "Cancel"))
				{
					Debug.Log("Fetching dialogue data");
					LoadDialogueData(jsonExtension);
					ShowDialogueData(createdCollection);
				}


				/*
				if (m_CollectionTypePopup.value == typeof(AssetTableCollection))
				{
					createdCollection =
						LocalizationEditorSettings.CreateAssetTableCollection(m_TableCollectionName.value,
							assetDirectory, locales);
				}
				*/
			};
		}

		private void LoadDialogueData(JsonExtension extension)
		{
			var google = new JsonTableSync
			{
				TableId = extension.TableId
			};
			google.PullIntoStringTableCollection(extension.TargetCollection as StringTableCollection, extension.Fields,
				extension.RemoveMissingPulledKeys, TaskReporter.CreateDefaultReporter(), true);
		}

		private void ShowDialogueData(StringTableCollection collection)
		{
			// Select the root asset and open the table editor window.
			Selection.activeObject = collection;
			LocalizationTablesWindow.ShowWindow(collection);
		}
	}
}
