using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using TMPro;

namespace StoryTime.Editor.UI
{
	using ResourceManagement;
	using FirebaseService.Settings;
	using Configurations.ScriptableObjects;

	public class DatabaseSyncWindow : EditorWindow
    {
	    const string WindowTitle = "StoryTime Settings";
	    private const string GameConfigField = "game-config-field";

	    public static void OpenWindow()
        {
	        var window = GetWindow<DatabaseSyncWindow>(false, WindowTitle, true);
	        window.titleContent = new GUIContent("StoryTime Settings", EditorIcons.LocalizationSettings.image);
	        window.Show();
        }

        void OnEnable()
        {
	        var asset = Resources.GetTemplateAsset(nameof(DatabaseSyncWindow));
	        var styles = Resources.GetStyleAsset(nameof(DatabaseSyncWindow));

	        asset.CloneTree(rootVisualElement);
	        rootVisualElement.styleSheets.Add(styles);
	        Init();
        }

        void Init()
        {
	        var root = rootVisualElement;

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

	        var dialogueConfigFile = StoryTime.ResourceManagement.HelperClass.GetAsset<GameSettingConfigSO>("");

            // First get the config instance id if existing
            var field = root.Q<ObjectField>("config-field");
            field.objectType = typeof(FirebaseConfigSO);
            field.RegisterValueChangedCallback(OnConfigFileChanged);

            var configFile = StoryTime.ResourceManagement.HelperClass.GetAsset<FirebaseConfigSO>(FirebaseConfigSO.SettingsPath);
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

		            // configFile.DataPath = assetDirectory;
		            SaveConfig(configFile);
	            };
            }

            // First get the dialogue config instance id if existing
            var dialogueConfigField = root.Q<ObjectField>(GameConfigField);
            dialogueConfigField.objectType = typeof(GameSettingConfigSO);
            dialogueConfigField.RegisterValueChangedCallback(OnDialogueFileChanged);

            var fontField = root.Q<ObjectField>("dialogue-font-field");
            fontField.objectType = typeof(TMP_FontAsset);

            if (dialogueConfigFile)
            {
	            dialogueConfigField.value = dialogueConfigFile;
	            root.Q<VisualElement>("GameSettings").style.display = DisplayStyle.Flex;
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

        /*
        void TabSelected(int idx)
        {
            if (SelectedTab == idx)
                return;

            m_TabToggles[SelectedTab].SetValueWithoutNotify(false);
            m_TabPanels[SelectedTab].style.display = DisplayStyle.None;

            m_TabToggles[idx].SetValueWithoutNotify(true);
            m_TabPanels[idx].style.display = DisplayStyle.Flex;

            SelectedTab = idx;
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            // menu.AddItem(new GUIContent("Import/XLIFF Directory"), false, Plugins.XLIFF.MenuItems.ImportXliffDirectory);

            // int idx = m_TabPanels.FindIndex(p => p is EditAssetTables);

            // var panel = m_TabPanels[idx] as EditAssetTables;
            // if (SelectedTab != idx)
                // return;

            // var selectedCollection = panel.SelectedCollection as StringTableCollection;
            // if (selectedCollection != null)
            // {
                // menu.AddItem(new GUIContent("Import/XLIFF File"), false, () => Plugins.XLIFF.MenuItems.ImportIntoCollection(new MenuCommand(selectedCollection)));
                // menu.AddItem(new GUIContent("Import/CSV File"), false, () => Plugins.CSV.MenuItems.ImportCollection(new MenuCommand(selectedCollection)));
                // menu.AddItem(new GUIContent("Export/XLIFF"), false, () => Plugins.XLIFF.MenuItems.ExportCollection(new MenuCommand(selectedCollection)));
                // menu.AddItem(new GUIContent("Export/CSV"), false, () => Plugins.CSV.MenuItems.ExportCollection(new MenuCommand(selectedCollection)));
                // menu.AddItem(new GUIContent("Export/CSV(With Comments)"), false, () => Plugins.CSV.MenuItems.ExportCollectionWithComments(new MenuCommand(selectedCollection)));
            // }
        }
        */

        void OnConfigFileChanged(ChangeEvent<UnityEngine.Object> evt)
        {
	        var config = evt.newValue as FirebaseConfigSO;
	        if (config != null)
	        {
		        // then get the config file is selected
		        rootVisualElement.Q<VisualElement>("rowSettings").style.display = DisplayStyle.Flex;
		        rootVisualElement.Bind(new SerializedObject(config));
		        // FirebaseSyncConfig.SelectedConfig = AssetDatabase.GetAssetPath(config);
	        }
	        else
	        {
		        rootVisualElement.Q<VisualElement>("rowSettings").style.display = DisplayStyle.None;
		        // FirebaseSyncConfig.SelectedConfig = "";
	        }
        }

        void OnDialogueFileChanged(ChangeEvent<UnityEngine.Object> evt)
        {
	        var config = evt.newValue as GameSettingConfigSO;
	        if (config != null)
	        {
		        // then get the config file is selected
		        rootVisualElement.Q<VisualElement>("gameSettings").style.display = DisplayStyle.Flex;
		        rootVisualElement.Bind(new SerializedObject(config));
		        // FirebaseSyncConfig.SelectedDialogueConfig = AssetDatabase.GetAssetPath(config);
	        }
	        else
	        {
		        rootVisualElement.Q<VisualElement>("gameSettings").style.display = DisplayStyle.None;
		        // FirebaseSyncConfig.SelectedDialogueConfig = "";
	        }
        }

        void SaveConfig(FirebaseConfigSO configFile, GameSettingConfigSO dialogueSettingConfig = null)
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
