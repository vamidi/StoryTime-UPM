using UnityEngine;
using UnityEngine.Events;

using TMPro;

namespace DatabaseSync
{
	[CreateAssetMenu(menuName = "DatabaseSync/Configurations/Dialogue Setting File", fileName = "DialogueSettingConfig")]
	public class DialogueSettingConfig : ScriptableObject
	{
		public TMP_FontAsset Font { get => font; set => font = value; }
		public int CharFontSize { get => charFontSize; set => charFontSize = value; }
		public int DialogueFontSize { get => dialogueFontSize; set => dialogueFontSize = value; }
		public bool ShowDialogueAtOnce { get => showDialogueAtOnce; set => showDialogueAtOnce = value; }
		public bool AnimatedText { get => animatedText; set => animatedText = value; }
		public bool AutoResize { get => autoResize; set => autoResize = value; }

		[SerializeField, Tooltip("Default font we play for the dialogues.")] private TMP_FontAsset font;
		[SerializeField, Range(15, 250)] private int charFontSize = 15;
		[SerializeField, Range(15, 250)] private int dialogueFontSize = 15;
		[SerializeField, Tooltip("Automatic resize the text when there is less or lots of text on screen.")] private bool autoResize = true;
		[SerializeField, Tooltip("Whether to show the dialogue all at once.")] private bool showDialogueAtOnce = true;
		[SerializeField, Tooltip("Show text with animation")] private bool animatedText = true;

	}
}
