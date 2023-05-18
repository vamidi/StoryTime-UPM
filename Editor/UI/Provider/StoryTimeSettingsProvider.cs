using System;
using System.Collections.Generic;
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
using UnityEngine.Localization;
using UnityEditor.Localization.UI;

using StoryTime.Database;
using StoryTime.Utils.Extensions;
using StoryTime.Utils.Configurations;
using UnityEditor.Localization.Reporting;
using StoryTime.Database.ScriptableObjects;
using StoryTime.Editor.Localization.Plugins.JSON.Fields;

// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.UI
{
	using Localization.Plugins.JSON;

	// using FirebaseService.Database;
	using FirebaseService.Settings;
	using Configurations.ScriptableObjects;

	public class StoryTimeSettingsProvider : SettingsProvider
	{
		internal const string SettingsPath = "Project/StoryTime System Package";

		protected class LocalizationData
		{
			/// <summary>
			/// Table name value from the <see cref="TableDatabase"/>
			/// </summary>
			public string Table = "";

			/// <summary>
			/// Localized table names from <see cref="StringTableCollection"/>
			/// </summary>
			public string[] TableNames;

			/// <summary>
			/// The columns that we should retrieve inside the <see cref="TableSO"/>
			/// </summary>
			public string[] Columns;
		}

		private SerializedObject _settings;
		private SerializedObject _globalSettings;
		private SerializedObject _gameSettings;

		// ReSharper disable once InconsistentNaming
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

		public static void Open()
		{
			SettingsService.OpenProjectSettings(SettingsPath);
		}

		StoryTimeSettingsProvider() : base(SettingsPath, SettingsScope.Project)
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
			_settings = FirebaseConfigSO.GetSerializedSettings();
			_globalSettings = GlobalSettingsSO.GetSerializedSettings();

			var gameConfig = ((GlobalSettingsSO)_globalSettings.targetObject).GameSettings;
			if (gameConfig)
			{
				_gameSettings = new SerializedObject(gameConfig);
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

		protected virtual List<LocalizationData> GetLocalizationData()
		{
			return new ()
			{
				new LocalizationData
				{
					Table = "Dialogues",
					TableNames = new [] { "Dialogues" },
					Columns = new [] { "text" }
				},

				new LocalizationData
				{
					Table = "Characters",
					TableNames = new [] { "Character Names" },
					Columns = new [] { "name" }
				},

				new LocalizationData
				{
					Table = "Stories",
					TableNames = new [] { "Story Titles", "Story Descriptions" },
					Columns = new [] { "title", "description" }
				},

				new LocalizationData
				{
					Table = "Items",
					TableNames = new [] { "Item Names", "Item Descriptions" },
					Columns = new [] { "name", "description" }
				},

				new LocalizationData
				{
					Table = "Enemies",
					TableNames = new [] { "Enemies" },
					Columns = new [] { "name" }
				},

				new LocalizationData
				{
					Table = "Tasks",
					TableNames = new [] { "Tasks" },
					Columns = new [] { "description" }
				},

				new LocalizationData
				{
					Table = "Classes",
					TableNames = new [] { "Classes" },
					Columns = new [] { "className" }
				},

				new LocalizationData
				{
					Table = "Skills",
					TableNames = new [] { "Skill Names", "Skill Descriptions" },
					Columns = new [] { "name", "description" }
				},

				new LocalizationData
				{
					Table = "Equipments",
					TableNames = new [] { "Equipment Names", "Equipment Descriptions" },
					Columns = new [] { "name", "description" }
				},

				new LocalizationData
				{
					Table = "EffectTypes",
					TableNames = new [] { "Effect Types" },
					Columns = new [] { "name" }
				},
				new LocalizationData
				{
					Table = "EnemyCategories",
					TableNames = new [] { "Enemy Categories" },
					Columns = new [] { "name" }
				},

				/* TODO what do we want to do for equipment types
				new LocalizationData
				{
					Table = "EquipmentTypes",
					TableNames = new [] { "Dialogues" },
					Columns = new [] { "text" }
				},
				*/

				new LocalizationData
				{
					Table = "ItemTypes",
					TableNames = new [] { "Item Types" },
					Columns = new [] { "name" }
				},

				new LocalizationData
				{
					Table = "NonPlayableCharacters",
					TableNames = new [] { "NPC Names", "NPC Descriptions" },
					Columns = new [] { "name", "description" }
				},

				new LocalizationData
				{
					Table = "Attributes",
					TableNames = new [] { "Attribute Names" },
					Columns = new [] { "paramName" }
				},

				new LocalizationData
				{
					Table = "QuestTypes",
					TableNames = new [] { "Quest Types" },
					Columns = new [] { "name" }
				},

				new LocalizationData
				{
					Table = "RewardTypes",
					TableNames = new [] { "Reward Types" },
					Columns = new [] { "name" }
				},

				// TODO add descriptions for the shop owners
				new LocalizationData
				{
					Table = "Shops",
					TableNames = new [] { "Shop Names" },
					Columns = new [] { "name" }
				},

				new LocalizationData
				{
					Table = "TaskCompletionTypes",
					TableNames = new [] { "Task Completion Types" },
					Columns = new [] { "name" }
				}
			};
		}

		// TODO refactor this because this is duplicate code from DatabaseSyncWindow
		private void Initialize(VisualElement root)
		{
			// Grab the root settings elements
			var globalSettingsField = root.Q<VisualElement>("globalSettings");

			// First get the settings field and bind it to the config file.
			var globalConfigField = root.Q<ObjectField>("global-config-field");
			globalConfigField.objectType = typeof(GlobalSettingsSO);
			globalConfigField.value = _globalSettings.targetObject;
			globalConfigField.Bind(_globalSettings);

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
						FirebaseSettings = ResourceManagement.HelperClass.GetAssetPath(_settings.targetObject),
						GameSettings = ResourceManagement.HelperClass.GetAssetPath(_gameSettings.targetObject),
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
			field.value = _settings.targetObject;
			field.Bind(_settings);

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
						GlobalSettings = ResourceManagement.HelperClass.GetAssetPath(_globalSettings.targetObject),
						FirebaseSettings = ResourceManagement.HelperClass.GetAssetPath(config),
						GameSettings = ResourceManagement.HelperClass.GetAssetPath(_gameSettings.targetObject),
					});
				}
				else
				{
					field.Unbind();
					// FirebaseSyncConfig.SelectedConfig = "";
				}
			});

			// First get the dialogue config instance id if existing
			var dialogueConfigField = root.Q<ObjectField>("game-config-field");
			dialogueConfigField.objectType = typeof(GameSettingConfigSO);

			if (_gameSettings != null && _gameSettings.targetObject)
			{
				dialogueConfigField.value = _gameSettings.targetObject;
				dialogueConfigField.Bind(_gameSettings);
			}

			dialogueConfigField.RegisterValueChangedCallback((evt) =>
			{
				var config = evt.newValue as GameSettingConfigSO;
				if (config != null)
				{
					// then get the config file is selected
					((GlobalSettingsSO) _globalSettings.targetObject).GameSettings = config;

					var serializedConfig = new SerializedObject(config);
					_gameSettings = serializedConfig;

					dialogueConfigField.Bind(serializedConfig);
					dialogueConfigField.style.display = DisplayStyle.Flex;
					// FirebaseSyncConfig.SelectedDialogueConfig = AssetDatabase.GetAssetPath(config);

					PathLocations.SavePathLocations(new PathLocations
					{
						GlobalSettings = ResourceManagement.HelperClass.GetAssetPath(_globalSettings.targetObject),
						FirebaseSettings = ResourceManagement.HelperClass.GetAssetPath(_settings.targetObject),
						GameSettings = ResourceManagement.HelperClass.GetAssetPath(config),
					});

					_globalSettings.ApplyModifiedProperties();
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

		private void SaveConfig(FirebaseConfigSO configFile, GameSettingConfigSO dialogueSettingConfig = null)
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

			SerializedProperty dataPathProp = _globalSettings.FindProperty("dataPath");
			root.Q<TextField>("data-path-field").BindProperty(dataPathProp);

			root.Q<Button>("btn-choose-path").clickable.clicked += () =>
			{
				var assetDirectory = EditorUtility.SaveFolderPanel("Data location", Application.dataPath, "");
				if (string.IsNullOrEmpty(assetDirectory))
					return;

				dataPathProp.stringValue = assetDirectory;
				_globalSettings.ApplyModifiedProperties();
				// SaveConfig(configFile);
			};

			// configure the buttons
			root.Q<Button>("btn-save").clickable.clicked += () => SaveConfig(_settings.targetObject as FirebaseConfigSO,
				_gameSettings.targetObject as GameSettingConfigSO);

			// Dialogue Settings
			SerializedProperty fontProp = _gameSettings.FindProperty("font");
			SerializedProperty autoResizeProp = _gameSettings.FindProperty("autoResize");
			SerializedProperty animatedTextProp = _gameSettings.FindProperty("animatedText");
			SerializedProperty showDialogueAtOnceProp = _gameSettings.FindProperty("showDialogueAtOnce");
			SerializedProperty voiceClipProp = _gameSettings.FindProperty("voiceClip");
			SerializedProperty punctuationClipProp = _gameSettings.FindProperty("punctuationClip");

			var toggle = root.Q<Toggle>("dialogue-resize-field");
			toggle.value = autoResizeProp.boolValue;
			toggle.RegisterValueChangedCallback(evt => autoResizeProp.boolValue = evt.newValue);

			toggle = root.Q<Toggle>("dialogue-animated-field");
			toggle.value = animatedTextProp.boolValue;
			toggle.RegisterValueChangedCallback(evt =>
			{
				animatedTextProp.boolValue = evt.newValue;
				_gameSettings.ApplyModifiedProperties();
			});

			toggle = root.Q<Toggle>("dialogue-show-field");
			toggle.value = showDialogueAtOnceProp.boolValue;
			toggle.RegisterValueChangedCallback(evt =>
			{
				showDialogueAtOnceProp.boolValue = evt.newValue;
				_gameSettings.ApplyModifiedProperties();
			});

			var audioClipField = root.Q<ObjectField>("dialogue-char-clip-field");
			audioClipField.objectType = typeof(AudioClip);
			audioClipField.value = voiceClipProp.objectReferenceValue;
			audioClipField.RegisterValueChangedCallback(evt =>
			{
				voiceClipProp.objectReferenceValue = evt.newValue as AudioClip;
				_gameSettings.ApplyModifiedProperties();
			});

			audioClipField = root.Q<ObjectField>("dialogue-punctuation-field");
			audioClipField.objectType = typeof(AudioClip);
			audioClipField.value = punctuationClipProp.objectReferenceValue;
			audioClipField.RegisterValueChangedCallback(evt =>
			{
				punctuationClipProp.objectReferenceValue = evt.newValue as AudioClip;
				_gameSettings.ApplyModifiedProperties();
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
					_gameSettings.ApplyModifiedProperties();
				}
			});

			// Firebase settings

			SerializedProperty databaseUrlProp = _settings.FindProperty("databaseURL");
			SerializedProperty storageBucketProp = _settings.FindProperty("storageBucket");
			SerializedProperty projectIDProp = _settings.FindProperty("projectID");
			// SerializedProperty useServerProp = settings.FindProperty("useServer");

			root.Q<TextField>("database-url-field").BindProperty(databaseUrlProp);
			root.Q<TextField>("storage-bucket-field").BindProperty(storageBucketProp);

			var dropdown = root.Q<DropdownField>("project-id-field");

			var config = _settings.targetObject as FirebaseConfigSO;
			if (config != null && !config.Projects.IsNullOrEmpty())
			{
				dropdown.value = config.Projects[projectIDProp.stringValue];
				dropdown.choices = config.Projects.Values.ToList();
				dropdown.RegisterValueChangedCallback(evt =>
				{
					var selected = config.Projects.FirstOrDefault(x => x.Value == evt.newValue).Key;
					projectIDProp.stringValue = selected;
					_settings.ApplyModifiedProperties();
				});
			}

			// Game settings
			PopulateLocalizationFields(root);
		}

		private void PopulateLocalizationFields(VisualElement root)
		{
			// get the array for the of the prop
			SerializedProperty tableIdsProp = _gameSettings.FindProperty("tableIds");

			// loop through all the localization data
			var localizedData = GetLocalizationData();
			foreach (var ld in localizedData)
			{
				// get the table
				TableSO tableSo = TableDatabase.Get.GetTableByName(ld.Table.LcFirst());

				if (tableSo == null)
				{
					continue;
				}

				var dropdown = root.Q<DropdownField>($"{ld.Table.LcFirst()}-fetch-field");

				if (dropdown == null)
				{
					continue;
				}

				// if (dropdown == null)
				// {
				// 	throw new ArgumentNullException($"Can't find field {ld.LcFirst()}");
				// }

				dropdown.value = tableSo ? tableSo.Metadata.title.UcFirst() : "";
				dropdown.choices = localizedData.Select(t => t.Table.UcFirst()).ToList();
				dropdown.RegisterValueChangedCallback(_ =>
				{
					// var toLower = evt.newValue.LcFirst();
					// TableSO table = TableDatabase.Get.GetTableByName(toLower);
					if (!tableSo)
					{
						return;
					}

					if (tableIdsProp.arraySize > 0)
					{
						for (int i = 0; i < tableIdsProp.arraySize; i++)
						{
							// If we found the table id already don't add it to the array.
							if (tableIdsProp.GetArrayElementAtIndex(i).stringValue == tableSo.ID)
								return;
						}
					}

					tableIdsProp.arraySize++;
					var newEntry = tableIdsProp.GetArrayElementAtIndex(tableIdsProp.arraySize - 1);
					newEntry.stringValue = tableSo.ID;
					_gameSettings.ApplyModifiedProperties();
				});
			}

			// TODO make this async if it takes a long time
			var fetchDialogueBtn = root.Q<Button>("btn-fetch-data");
			fetchDialogueBtn.clickable.clicked += () =>
			{
				if (!Directory.Exists($"{Application.dataPath}/Localization"))
				{
					Debug.Log("Creating localization folder");
					Directory.CreateDirectory($"{Application.dataPath}/Localization");
				}


				foreach (var ld in localizedData)
				{
					Debug.AssertFormat(
						ld.TableNames.Length == ld.Columns.Length, "TableNames and Columns are not the same length {0}, {1}",
						ld.TableNames.Length,
						ld.Columns.Length
					);

					TableSO tableSo = TableDatabase.Get.GetTableByName(ld.Table.LcFirst());

					if (tableSo == null)
					{
						Debug.LogFormat("Cant find table {0} in database", ld.Table);
						continue;
					}

					int ldColumnIndex = 0;
					EditorUtility.DisplayProgressBar("Importing Localization data", $"Importing {ld.Table}", (float)ldColumnIndex / localizedData.Count);
					foreach (var tableName in ld.TableNames)
					{
						ReadOnlyCollection<Locale> locales = LocalizationEditorSettings.GetLocales();
						StringTableCollection createdCollection =
							LocalizationEditorSettings.GetStringTableCollection(tableName);

						if (createdCollection == null)
						{
							// Create at location
							createdCollection = LocalizationEditorSettings.CreateStringTableCollection(tableName, $"{Application.dataPath}/Localization/{tableName}", locales);
						}

						var currentExtensions = createdCollection.Extensions;
						if (currentExtensions.IsNullOrEmpty() || !(currentExtensions.First(c => c.GetType() == typeof(JsonExtension)) is JsonExtension))
						{
							var jsonExtension = Activator.CreateInstance(typeof(JsonExtension)) as JsonExtension;

							if (jsonExtension == null)
							{
								throw new ArgumentNullException(nameof(jsonExtension));
							}

							jsonExtension.TableName = tableName;
							jsonExtension.TableId = tableSo.ID;

							string columnName = ld.Columns[ldColumnIndex];
							var columns = FieldMapping.CreateDefaultMapping(columnName);
							jsonExtension.Fields = columns.AsReadOnly();

							createdCollection.AddExtension(jsonExtension);

							Debug.Log($"Fetching {tableName} data");
							LoadLocalizationData(jsonExtension);
							// ShowDialogueData(createdCollection);
							continue;
						}

/*
						if (EditorUtility.DisplayDialog("Dialogue data found!",
							    "Are you sure you want to replace the existing data?", "Overwrite", "Cancel"))
						{
							Debug.Log("Fetching dialogue data");
							LoadLocalizationData(jsonExtension);
							// ShowDialogueData(createdCollection);
						}
*/
						ldColumnIndex++;
					}
					EditorUtility.ClearProgressBar();
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

		private void LoadLocalizationData(JsonExtension extension)
		{
			var sync = new JsonTableSync
			{
				TableId = extension.TableId
			};
			sync.PullIntoStringTableCollection(extension.TargetCollection as StringTableCollection, extension.Fields,
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
