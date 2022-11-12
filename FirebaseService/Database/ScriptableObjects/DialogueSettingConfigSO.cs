using UnityEngine;

using TMPro;
using UnityEditor;

namespace StoryTime.Configurations.ScriptableObjects
{
	using ResourceManagement;

	[CreateAssetMenu(menuName = "StoryTime/Configurations/Dialogue Setting File", fileName = "DialogueSettingConfig")]
	// ReSharper disable once InconsistentNaming
	public class DialogueSettingConfigSO : ScriptableObject
	{
		public TMP_FontAsset Font { get => font; set => font = value; }
		public AudioClip PunctuationClip { get => punctuationClip; set => punctuationClip = value; }
		public AudioClip VoiceClip { get => voiceClip; set => voiceClip = value; }
		public int CharFontSize { get => charFontSize; set => charFontSize = value; }
		public int DialogueFontSize { get => dialogueFontSize; set => dialogueFontSize = value; }
		public bool ShowDialogueAtOnce { get => showDialogueAtOnce; set => showDialogueAtOnce = value; }
		public bool AnimatedText { get => animatedText; set => animatedText = value; }
		public bool AutoResize { get => autoResize; set => autoResize = value; }

		[SerializeField, Tooltip("Default font we play for the dialogues.")] private TMP_FontAsset font;
		[SerializeField, Tooltip("Dialogue character punctuation sound")] private AudioClip punctuationClip;
		[SerializeField, Tooltip("Dialogue character voice sound")] private AudioClip voiceClip;
		[SerializeField, Range(15, 250)] private int charFontSize = 15;
		[SerializeField, Range(15, 250)] private int dialogueFontSize = 15;
		[SerializeField, Tooltip("Automatic resize the text when there is less or lots of text on screen.")] private bool autoResize = true;
		[SerializeField, Tooltip("Whether to show the dialogue all at once.")] private bool showDialogueAtOnce = true;
		[SerializeField, Tooltip("Show text with animation")] private bool animatedText = true;

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public static DialogueSettingConfigSO FetchDialogueSetting()
		{
#if UNITY_EDITOR
			var path = EditorPrefs.GetString("StoryTime-Window-Dialogue-Settings-Config", "");
			var configFile =
				HelperClass.GetAsset<DialogueSettingConfigSO>(AssetDatabase.GUIDToAssetPath(path));
#else
			// TODO make it work outside the unity editor.
			var configFile = "";
#endif
			return configFile;
		}
	}
}
