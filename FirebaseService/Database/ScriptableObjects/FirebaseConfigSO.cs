using System;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using TMPro;

// ReSharper disable once CheckNamespace
namespace StoryTime.Configurations.ScriptableObjects
{
	using ResourceManagement;
	using FirebaseService.Database;

	/// <summary>
	/// Configuration for connecting to a Google Sheet.
	/// </summary>
	public interface ITableService { }

	[CreateAssetMenu(menuName = "StoryTime/Configurations/Config File", fileName = "FirebaseConfig")]
	// ReSharper disable once InconsistentNaming
	public class FirebaseConfigSO : ScriptableObject, ITableService
	{
		public string Url => storyTimeUrl;

		public string Email => email;

		internal string Password => password;

		public string DatabaseURL => databaseURL;
		internal string ProjectID => projectID;

		public string FirebaseProjectId => firebaseProjectId;

		internal string AppId => appId;

		internal string ApiKey => apiKey;

		public string MessageSenderId => messageSenderId;

		public string StorageBucket => storageBucket;

		public bool Authentication => useServer;

		/// <summary>
		///
		/// </summary>
		[SerializeField] private string storyTimeUrl;

		/// <summary>
		/// Database url to fetch data from
		/// You can retrieve it from your JSON file.
		/// </summary>
		[SerializeField] private string email = "";

		/// <summary>
		/// Database url to fetch data from
		/// You can retrieve it from your JSON file.
		/// </summary>
		[SerializeField] private string password = "";

		/// <summary>
		/// Database url to fetch data from
		/// You can retrieve it from your JSON file.
		/// </summary>
		[SerializeField] private string databaseURL = "";

		/// <summary>
		/// Firebase project id.
		/// You can retrieve the id from the json file.
		/// </summary>
		[SerializeField] private string firebaseProjectId = "";

		/// <summary>
		/// Firebase app id.
		/// </summary>
		[SerializeField] private string appId = "";

		/// <summary>
		/// Firebase api key
		/// </summary>
		[SerializeField] private string apiKey = "";

		/// <summary>
		/// Message sender id
		/// </summary>
		[SerializeField] private string messageSenderId = "";

		/// <summary>
		/// Storage bucket
		/// </summary>
		[SerializeField] private string storageBucket = "";

		/// <summary>
		/// Project we want to load the data from
		/// </summary>
		[SerializeField] private string projectID = "";

		/// <summary>
		/// The location where to store the JSON data locally.
		/// </summary>
		[SerializeField] public string dataPath = "";

		/// <summary>
		///
		/// </summary>
		[SerializeField] public bool useServer;

#if UNITY_EDITOR
		internal static FirebaseConfigSO FindSettings()
		{
			var configFile = HelperClass.GetAsset<FirebaseConfigSO>(FirebaseSyncConfig.SelectedConfig);
			if (configFile == null)
			{
				Debug.LogWarning($"{nameof(configFile)} must not be null.");
			}

			return configFile;
		}
#else
		internal static void Fetch(Action<AsyncOperationHandle<FirebaseConfigSO>> callback)
		{
			// TODO FIX me
			var configFile = HelperClass.GetFileFromAddressable<FirebaseConfigSO>("DatabaseConfig");
			configFile.Completed += callback;
		}
#endif

		internal static FirebaseConfigSO GetOrCreateSettings() {
			var settings = FindSettings();
			if (settings == null) {
				settings = ScriptableObject.CreateInstance<FirebaseConfigSO>();
				AssetDatabase.CreateAsset(settings, "Assets");
				AssetDatabase.SaveAssets();
			}
			return settings;
		}

		internal static SerializedObject GetSerializedSettings() {
			return new SerializedObject(GetOrCreateSettings());
		}
	}

	// Register a SettingsProvider using UIElements for the drawing framework:
	static class MyCustomSettingsUIElementsRegister {
		[SettingsProvider]
		public static SettingsProvider CreateMyCustomSettingsProvider() {
			// First parameter is the path in the Settings window.
			// Second parameter is the scope of this setting: it only appears in the Settings window for the Project scope.
			var provider = new SettingsProvider("Project/MyCustomUIElementsSettings", SettingsScope.Project) {
				label = "StoryTime",
				// activateHandler is called when the user clicks on the Settings item in the Settings window.
				activateHandler = (_, rootElement) => {
					// var settings = FirebaseConfigSO.GetSerializedSettings();

					var asset = Editor.UI.Resources.GetTemplateAsset("DatabaseSyncWindow");
					var styles = Editor.UI.Resources.GetStyleAsset("DatabaseSyncWindow");

					asset.CloneTree(rootElement);
					rootElement.styleSheets.Add(styles);
					Init(rootElement);

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
				},
			};

			return provider;
		}

		// TODO refactor this because this is duplicate code from DatabaseSyncWindow
		private static void Init(VisualElement root)
        {
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

	        var dialogueConfigFile = HelperClass.GetAsset<DialogueSettingConfigSO>(FirebaseSyncConfig.SelectedDialogueConfig);

            // First get the config instance id if existing
            var field = root.Q<ObjectField>("config-field");
            field.objectType = typeof(FirebaseConfigSO);

            field.RegisterValueChangedCallback((evt) =>
            {
	            var config = evt.newValue as FirebaseConfigSO;
	            if (config != null)
	            {
		            // then get the config file is selected
		            root.Q<VisualElement>("rowSettings").style.display = DisplayStyle.Flex;
		            root.Bind(new SerializedObject(config));
		            FirebaseSyncConfig.SelectedConfig = AssetDatabase.GetAssetPath(config);
	            }
	            else
	            {
		            root.Q<VisualElement>("rowSettings").style.display = DisplayStyle.None;
		            FirebaseSyncConfig.SelectedConfig = "";
	            }
            });

            if (FirebaseSyncConfig.SelectedConfig != String.Empty)
            {
	            var configFile = HelperClass.GetAsset<FirebaseConfigSO>(FirebaseSyncConfig.SelectedConfig);
	            if (configFile)
	            {
		            field.value = configFile;
		            root.Q<VisualElement>("rowSettings").style.display = DisplayStyle.Flex;
		            root.Bind(new SerializedObject(field.value));

		            // configure the buttons
		            root.Q<Button>("btn-save").clickable.clicked += () => SaveConfig(configFile, dialogueConfigFile);

		            root.Q<Button>("btn-choose-path").clickable.clicked += () =>
		            {
			            var assetDirectory = EditorUtility.SaveFolderPanel("Data location", Application.dataPath, "");
			            if (string.IsNullOrEmpty(assetDirectory))
				            return;

			            configFile.dataPath = assetDirectory;
			            SaveConfig(configFile);
		            };
	            }
            }

            // First get the dialogue config instance id if existing
            var dialogueConfigField = root.Q<ObjectField>("dialogue-config-field");
            dialogueConfigField.objectType = typeof(DialogueSettingConfigSO);
            dialogueConfigField.RegisterValueChangedCallback((evt) =>
            {
	            var config = evt.newValue as DialogueSettingConfigSO;
	            if (config != null)
	            {
		            // then get the config file is selected
		            root.Q<VisualElement>("dialogueSettings").style.display = DisplayStyle.Flex;
		            root.Bind(new SerializedObject(config));
		            FirebaseSyncConfig.SelectedDialogueConfig = AssetDatabase.GetAssetPath(config);
	            }
	            else
	            {
		            root.Q<VisualElement>("dialogueSettings").style.display = DisplayStyle.None;
		            FirebaseSyncConfig.SelectedDialogueConfig = "";
	            }
            });

            var fontField = root.Q<ObjectField>("dialogue-font-field");
            fontField.objectType = typeof(TMP_FontAsset);

            if (FirebaseSyncConfig.SelectedDialogueConfig != String.Empty)
            {
	            if (dialogueConfigFile)
	            {
		            dialogueConfigField.value = dialogueConfigFile;
		            root.Q<VisualElement>("dialogueSettings").style.display = DisplayStyle.Flex;
		            root.Bind(new SerializedObject(dialogueConfigField.value));

		            fontField.RegisterValueChangedCallback((evt) =>
		            {
			            var newValue = evt.newValue as TMP_FontAsset;
			            if (newValue != null)
			            {
				            dialogueConfigFile.Font = newValue;
			            }
		            });

		            var toggle = root.Q<Toggle>("dialogue-resize-field");
		            toggle.RegisterValueChangedCallback(evt => dialogueConfigFile.AutoResize = evt.newValue);

		            toggle = root.Q<Toggle>("dialogue-animated-field");
		            toggle.RegisterValueChangedCallback(evt => dialogueConfigFile.AnimatedText = evt.newValue);

		            toggle = root.Q<Toggle>("dialogue-show-field");
		            toggle.RegisterValueChangedCallback(evt => dialogueConfigFile.ShowDialogueAtOnce = evt.newValue);

		            var audioClipField = root.Q<ObjectField>("dialogue-char-clip-field");
					audioClipField.objectType = typeof(AudioClip);

					audioClipField.value = dialogueConfigFile.VoiceClip;
					audioClipField.RegisterValueChangedCallback(evt => dialogueConfigFile.VoiceClip = evt.newValue as AudioClip);

					audioClipField = root.Q<ObjectField>("dialogue-punctuation-field");
		            audioClipField.objectType = typeof(AudioClip);

		            audioClipField.value = dialogueConfigFile.PunctuationClip;
		            audioClipField.RegisterValueChangedCallback(evt => dialogueConfigFile.PunctuationClip = evt.newValue as AudioClip);

		            //
			        // public int CharFontSize { get => charFontSize; set => charFontSize = value; }
					// public int DialogueFontSize { get => dialogueFontSize; set => dialogueFontSize = value; }
	            }

            }
        }

		private static void SaveConfig(FirebaseConfigSO configFile, DialogueSettingConfigSO dialogueSettingConfig = null)
		{
			Debug.Log("Saving Database Config file");
			EditorUtility.SetDirty(configFile);
			if (dialogueSettingConfig != null)
			{
				EditorUtility.SetDirty(dialogueSettingConfig);
			}

			AssetDatabase.SaveAssets();
		}
	}
}



