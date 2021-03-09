using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Events;

using TMPro;

namespace DatabaseSync.Components
{
	using Binary;
	using ResourceManagement.Util;

	public enum Emotion { Happy, Sad, Surprised, Angry };

	[Serializable] public class ActionEvent : UnityEvent<string> { }
	[Serializable] public class CharRevealEvent : UnityEvent<char> { }

	[Serializable] public class EmotionEvent : UnityEvent<Emotion> { }

	public class TMPAnimated : TextMeshProUGUI
	{
		[Header("Configuration")]
		public ActionEvent onAction;
		public EmotionEvent onEmotionChange;
		public CharRevealEvent onCharReveal;
		public bool useConfig = true;

		public int numCharactersFade = 3;
		public float charsPerSecond = 30;
		public float smoothSeconds = 0.75f;

		public UnityEvent allRevealed = new UnityEvent();
		public bool IsRevealing => _isRevealing;

		[SerializeField] private float speed = 10f;
		private readonly Dictionary<string, TagExpression> m_Expressions = new Dictionary<string, TagExpression>{
			{ "action", new TagExpression(@"<action=(.*?)>", @"(?<=<action=)(.*?)(?=>)") }, // Get action => these are things that We should fix
			{ "emotion", new TagExpression(@"<emotion=(.*?)>", @"(?<=<emotion=)(.*?)(?=>)") }, // Get emotion => these are things that We should fix
			{ "tmp_tag", new TagExpression(@"(?<=<(.*?)>)(.*?)(?=<\/\k<1>>)", "") }, // Get tags with value that should be handled by TMP
			{ "tmp_tag_value", new TagExpression(@"(?<=<(.*?)>)(.*?)(?=<\/\k<1>>)", "") } // Get tags with value that should be handled by TMP
	};

		private string _originalString = String.Empty;
		private string _adjustedString = String.Empty;

		private int _nRevealedCharacters;
		private bool _isRevealing;

		private DialogueSettingConfig m_Config;

		public void RestartWithText(string strText)
		{
			text = String.Empty;
			_nRevealedCharacters = 0;
			_originalString = strText;
			_isRevealing = false;

			// Replace the action with nothing
			_adjustedString = Regex.Replace(_originalString, m_Expressions["action"].Tag, "", RegexOptions.Singleline);

			// Replace the emotions with nothing
			_adjustedString = Regex.Replace(_adjustedString, m_Expressions["emotion"].Tag, "", RegexOptions.Singleline);

			// remove all the text.
			text = BuildPartiallyRevealedString(_adjustedString, keyCharIndex: -1, minIndex: 0, maxIndex: 0, fadeLength: 1, out _, out _);
		}

		public void ShowEverythingWithoutAnimation()
		{
			StopAllCoroutines();

			// TODO call all actions and emotions

			// Replace the action with nothing
			_originalString = Regex.Replace(_originalString, m_Expressions["action"].Tag, "", RegexOptions.Singleline);

			// Replace the emotions with nothing
			_originalString = Regex.Replace(_originalString, m_Expressions["emotion"].Tag, "", RegexOptions.Singleline);

			text = _originalString;
			_nRevealedCharacters = _originalString.Length;
			_isRevealing = false;

			allRevealed.Invoke();
		}

		public void ShowNextParagraphWithoutAnimation()
		{
			if (IsAllRevealed()) return;

			StopAllCoroutines();

			var paragraphEnd = GetNextParagraphEnd(_nRevealedCharacters);
			text = BuildPartiallyRevealedString(original: _originalString,
				keyCharIndex: paragraphEnd,
				minIndex: _nRevealedCharacters,
				maxIndex: paragraphEnd,
				fadeLength: 0,
				out _,
				out _);

			_nRevealedCharacters = paragraphEnd + 1;
			while (_nRevealedCharacters < _originalString.Length && _originalString[_nRevealedCharacters] == '\n')
				_nRevealedCharacters += 1;

			if (IsAllRevealed())
				allRevealed.Invoke();

			_isRevealing = false;
		}

		public void RevealNextParagraphAsync()
		{
			if (m_Config != null)
			{
				if (!m_Config.AnimatedText) // show character for character without animation
				{
					ShowNextParagraphWithoutAnimation();
					return;
				}

				if (m_Config.ShowDialogueAtOnce) // Show all dialogue at once
				{
					StopAllCoroutines();
					ShowEverythingWithoutAnimation();
					return;
				}
			}

			StartCoroutine(RevealNextParagraph());
		}

		public IEnumerator RevealNextParagraph()
		{
			if (IsAllRevealed() || _isRevealing) yield break;

			var paragraphEnd = GetNextParagraphEnd(_nRevealedCharacters);
			if (paragraphEnd < 0) yield break;

			_isRevealing = true;

			var keyChar = (float)(_nRevealedCharacters - numCharactersFade);
			var keyCharEnd = paragraphEnd;
			// var secondsElapsed = 0f;

			while (keyChar < keyCharEnd)
			{
				/*
				secondsElapsed += Time.deltaTime;
				if (secondsElapsed <= smoothSeconds)
					speed = Mathf.Lerp(0f, charsPerSecond, secondsElapsed / smoothSeconds);
				else
				{
					var secondsLeft = (keyCharEnd - keyChar) / charsPerSecond;
					if (secondsLeft < smoothSeconds)
						speed = Mathf.Lerp(charsPerSecond, 0.1f * charsPerSecond, 1f - secondsLeft / smoothSeconds);
				}
				*/

				keyChar = Mathf.MoveTowards(keyChar, keyCharEnd, speed * Time.deltaTime);
				// TODO add regular expression for TMP_pro to enable for example color.
				_adjustedString = BuildPartiallyRevealedString(original: _originalString,
					keyCharIndex: keyChar,
					minIndex: _nRevealedCharacters,
					maxIndex: paragraphEnd,
					fadeLength: numCharactersFade,
					newKeyIndex: out var newKeyChar,
					actionTag: out var actionTag);

				// Move towards the newest position when there is an action.
				keyChar = newKeyChar != -1 ? newKeyChar : keyChar;

				// Replace the action with nothing
				_adjustedString = Regex.Replace(_adjustedString, m_Expressions["action"].Tag, "", RegexOptions.Singleline);

				// Replace the emotions with nothing
				_adjustedString = Regex.Replace(_adjustedString, m_Expressions["emotion"].Tag, "", RegexOptions.Singleline);

				text = _adjustedString;

				yield return EvaluateTag(actionTag);

				yield return null;
			}

			_nRevealedCharacters = paragraphEnd + 1;

			while (_nRevealedCharacters < _originalString.Length && _originalString[_nRevealedCharacters] == '\n')
				_nRevealedCharacters += 1;

			if (IsAllRevealed())
				allRevealed.Invoke();

			_isRevealing = false;
		}

		public bool IsAllRevealed()
		{
			return _nRevealedCharacters >= _originalString.Length;
		}


		private int GetNextParagraphEnd(int startingFrom)
		{
			var paragraphEnd = _originalString.IndexOf('\n', startingFrom);
			if (paragraphEnd < 0 && startingFrom < _originalString.Length) paragraphEnd = _originalString.Length - 1;
			return paragraphEnd;
		}

		private string BuildPartiallyRevealedString(string original, float keyCharIndex, int minIndex, int maxIndex, int fadeLength, out int newKeyIndex, out string actionTag)
		{
			newKeyIndex = -1;
			actionTag = "";
			if (original.Length == 0)
			{
				return "";
			}

			var keyIndex = keyCharIndex;
			var lastFullyVisibleChar = Mathf.Max(Mathf.CeilToInt(keyIndex), minIndex - 1);
			var firstFullyInvisibleChar = (int) Mathf.Min(keyIndex + fadeLength, maxIndex) + 1;

			var revealed = original.Substring(0, lastFullyVisibleChar + 1);
			var unrevealed = original.Substring(firstFullyInvisibleChar);

			var sb = new StringBuilder();
			sb.Append(revealed);

			for (var i = lastFullyVisibleChar + 1; i < firstFullyInvisibleChar;)
			{
				if (original[i].CompareTo('<') == 0)
				{
					string GetTag(TagExpression expression, out int nKeyIndex)
					{
						nKeyIndex = -1;
						// split the whole text into parts based off the <> tags
						// even numbers in the array are text, odd numbers are tags
						// <action=>
						// first grab the value from the regular expression
						Match regexMatch = expression.MatchRegex(original, i);
						// the indices needs to match either
						if (regexMatch.Success && i == regexMatch.Index)
						{
							var endIndex = regexMatch.Index + regexMatch.Length;

							keyIndex = endIndex > original.Length ? endIndex - 1 : endIndex;
							firstFullyInvisibleChar = (int) Mathf.Min(keyIndex, maxIndex) + 1;
							unrevealed = original.Substring(firstFullyInvisibleChar);

							// jump to the end of the match
							// next iteration we have to start somewhere else
							i = nKeyIndex = firstFullyInvisibleChar;

							// Now grab the tag
							return expression.GetRegexValue(original, m => m.Value);
						}

						return String.Empty;
					}

					// see if we can get the action tag
					actionTag = GetTag(m_Expressions["action"], out newKeyIndex);

					if (string.IsNullOrEmpty(actionTag))
					{
						// Emotion tags
						var emotionExpression = GetTag( m_Expressions["emotion"], out newKeyIndex);

						if (!string.IsNullOrEmpty(emotionExpression))
						{
							onEmotionChange.Invoke((Emotion)Enum.Parse(typeof(Emotion), HelperClass.Capitalize(emotionExpression)));
						}
						else
						{
							// handle the tags for TMP
							GetTag(m_Expressions["tmp_tag"], out newKeyIndex);
							GetTag(m_Expressions["tmp_tag_value"], out newKeyIndex);
						}
					}
				}

				char c = original.Length == i ? original[i - 1] : original[i];
				var originalColorRGB = ColorUtility.ToHtmlStringRGB(color);
				var fadeAlpha = Mathf.RoundToInt(255 * (keyIndex - i) / fadeLength);
				sb.AppendFormat("<color=#{0}{1:X2}>{2}</color>", originalColorRGB, (byte)fadeAlpha, c);
				onCharReveal.Invoke(c);
				++i;
			}

			sb.AppendFormat("<color=#00000000>{0}</color>", unrevealed);
			return sb.ToString();
		}

		private WaitForSeconds EvaluateTag(string actionTag)
		{
			if (actionTag.Length > 0)
			{
				if (actionTag.StartsWith("speed"))
				{
					speed = float.Parse(tag);
				}
				else if (actionTag.StartsWith("pause"))
				{
					return new WaitForSeconds(float.Parse(tag));
				}
				else if (actionTag.StartsWith("action"))
				{
					onAction.Invoke(actionTag);
					Debug.Log(actionTag);
				}
			}
			return null;
		}

		protected override void Awake()
		{
			base.Awake();

			m_Config = TableBinary.FetchDialogueSetting();

			if (m_Config != null && useConfig)
			{
				font = m_Config.Font;
				enableAutoSizing = m_Config.AutoResize;
				if (enableAutoSizing)
				{
					// fontSizeMin = m_Config.minFontSize;
					// fontSizeMin = m_Config.maxFontSize;
				}

				fontSize = m_Config.DialogueFontSize;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			// reveal the current dialogue text.
			RevealNextParagraphAsync();
		}
	}
}
