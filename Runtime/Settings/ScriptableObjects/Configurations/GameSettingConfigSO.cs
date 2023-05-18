using System;
using System.Collections.Generic;
using StoryTime.Utils.Configurations;
using StoryTime.Utils.Extensions;
using TMPro;
using UnityEditor;

using UnityEngine;

using UnityEngine.Localization.Settings;
namespace StoryTime.Configurations.ScriptableObjects
{
	using ResourceManagement;

	[CreateAssetMenu(menuName = "StoryTime/Configurations/Dialogue Setting File", fileName = "DialogueSettingConfig")]
	// ReSharper disable once InconsistentNaming
	public class GameSettingConfigSO : ScriptableObject
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

		internal static GameSettingConfigSO GetOrCreateSettings()
		{
			PathLocations locations = PathLocations.FetchPathLocations();
			string destination = $"{SettingsPath}/GameSettingConfigSO.asset";

			// if we have a new location grab it
			if (!locations.GlobalSettings.IsNullOrEmpty())
			{
				destination = locations.GameSettings;
			}

			var settings = HelperClass.GetAsset<GameSettingConfigSO>(destination);

#if UNITY_EDITOR
			if (settings == null)
			{
				settings = HelperClass.CreateAsset<GameSettingConfigSO>(destination);
			}
#endif
			if (settings == null)
			{
				throw new ArgumentException("Settings cannot be null", nameof(settings));
			}


			/*
			var ls = CreateLocalizationAsset();
			if (ls == null)
				return;

			if (EditorUtility.DisplayDialog("Active localization settings",
				    "Do you wish to make this asset the active localization settings? The active localization settings will be included into any builds and preloaded at the start. This can be changed at 'Edit/Project Settings/Localization'",
				    "Yes",
				    "No"))
			{
				LocalizationEditorSettings.ActiveLocalizationSettings = ls;
				Selection.activeObject = ls;
			}
			*/

			return settings;
		}
#if UNITY_EDITOR
		internal static SerializedObject GetSerializedSettings() {
			return new SerializedObject(GetOrCreateSettings());
		}
#endif
	}
}
