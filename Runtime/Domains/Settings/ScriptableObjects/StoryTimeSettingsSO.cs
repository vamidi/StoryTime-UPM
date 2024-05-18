using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.Localization.Settings;

using TMPro;

using CandyCoded.env;
using StoryTime.ResourceManagement;

namespace StoryTime.Domains.Settings.ScriptableObjects
{
    [Serializable]
    public class APIConfigSettings
    {
        internal string ApiUrl => apiUrl;
        internal string ApiKey => apiKey;
        internal string ProjectID => projectID;
        
        /// <summary>
        /// Database url to fetch data from
        /// You can retrieve it from your JSON file.
        /// </summary>
        [SerializeField] private string apiUrl = "http://localhost:3000/api";

        /// <summary>
        /// API key
        /// </summary>
        [SerializeField] private string apiKey = "";

        /// <summary>
        /// Project we want to load the data from
        /// </summary>
        [SerializeField] private string projectID = "";

        public void OnEnable()
        {
            if (env.TryParseEnvironmentVariable("API_URL", out string url))
            {
                apiUrl = url;
            }

            if (env.TryParseEnvironmentVariable("API_KEY", out string key))
            {
                apiKey = key;
            }
        }
    }
    
    [Serializable]
    public class GameSettings
    {
        public TMP_FontAsset Font { get => font; set => font = value; }
		public AudioClip PunctuationClip { get => punctuationClip; set => punctuationClip = value; }
		public AudioClip VoiceClip { get => voiceClip; set => voiceClip = value; }
		public int CharFontSize { get => charFontSize; set => charFontSize = value; }
		public int DialogueFontSize { get => dialogueFontSize; set => dialogueFontSize = value; }
		public bool ShowDialogueAtOnce { get => showDialogueAtOnce; set => showDialogueAtOnce = value; }
		public bool AnimatedText { get => animatedText; set => animatedText = value; }
		public bool AutoResize { get => autoResize; set => autoResize = value; }

		internal const string SettingsPath = "Assets/Settings/StoryTime";

		[SerializeField] private LocalizationSettings localizationSettings;

		[SerializeField, Tooltip("Default font we play for the dialogues.")] private TMP_FontAsset font;
		[SerializeField, Tooltip("Dialogue character punctuation sound")] private AudioClip punctuationClip;
		[SerializeField, Tooltip("Dialogue character voice sound")] private AudioClip voiceClip;
		[SerializeField, Range(15, 250)] private int charFontSize = 15;
		[SerializeField, Range(15, 250)] private int dialogueFontSize = 15;
		[SerializeField, Tooltip("Automatic resize the text when there is less or lots of text on screen.")] private bool autoResize = true;
		[SerializeField, Tooltip("Whether to show the dialogue all at once.")] private bool showDialogueAtOnce = true;
		[SerializeField, Tooltip("Show text with animation")] private bool animatedText = true;

		[SerializeField, Tooltip("The selected table for fetching localized data")] private List<string> tableIds = new();
    }
    
    // ReSharper disable once InconsistentNaming
    public class StoryTimeSettingsSO : ScriptableObject
    {
        public const string CONFIG_NAME = "com.vamidicreations.storytime.settings";
        
        // API Settings
        public string ApiUrl => apiConfigSettings.ApiUrl;
        public string ApiKey => apiConfigSettings.ApiKey;
        public string ProjectID => apiConfigSettings.ProjectID;
        
        // Game Settings
        public TMP_FontAsset Font { get => gameSettings.Font; set => gameSettings.Font = value; }
        public AudioClip PunctuationClip { get => gameSettings.PunctuationClip; set => gameSettings.PunctuationClip = value; }
        public AudioClip VoiceClip { get => gameSettings.VoiceClip; set => gameSettings.VoiceClip = value; }
        public int CharFontSize { get => gameSettings.CharFontSize; set => gameSettings.CharFontSize = value; }
        public int DialogueFontSize { get => gameSettings.DialogueFontSize; set => gameSettings.DialogueFontSize = value; }
        public bool ShowDialogueAtOnce { get => gameSettings.ShowDialogueAtOnce; set => gameSettings.ShowDialogueAtOnce = value; }
        public bool AnimatedText { get => gameSettings.AnimatedText; set => gameSettings.AnimatedText = value; }
        public bool AutoResize { get => gameSettings.AutoResize; set => gameSettings.AutoResize = value; }
        
        internal ReadOnlyDictionary<string, string> Projects => new(projects);

        [SerializeField] private APIConfigSettings apiConfigSettings = new ();
        
        [SerializeField] private GameSettings gameSettings = new ();

#if UNITY_EDITOR
        [SerializeField] private SerializableDictionary<string, string> projects = new();
        
        internal void AddProject(string projectID, string name)
        {
	        if (!projects.TryAdd(projectID, name))
	        {
		        Debug.LogWarning($"Project with ID {projectID} already exists.");
	        }
        }
#endif

        private void OnEnable()
        {
            apiConfigSettings.OnEnable();
        }
    }
}