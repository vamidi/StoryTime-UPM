using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

namespace DatabaseSync.Components
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextRevealer : MonoBehaviour
	{
		[Header("Configuration")]
		public DialogueSettingConfig dialogueSettings;

		public int numCharactersFade = 3;
		public float charsPerSecond = 30;
		public float smoothSeconds = 0.75f;

		public UnityEvent allRevealed = new UnityEvent();

		private TextMeshProUGUI _text;
		private string _originalString = String.Empty;
		private int _nRevealedCharacters;
		private bool _isRevealing;

		public bool IsRevealing => _isRevealing;

		public void OnEnable()
		{
			// reveal the current dialogue text.
			RevealNextParagraphAsync();
		}

		public void RestartWithText(string strText)
		{
			_nRevealedCharacters = 0;
			_originalString = strText;
			_text.text = BuildPartiallyRevealedString(_originalString, keyCharIndex: -1, minIndex: 0, maxIndex: 0, fadeLength: 1);
		}

		public void ShowEverythingWithoutAnimation()
		{
			StopAllCoroutines();

			_text.text = _originalString;
			_nRevealedCharacters = _originalString.Length;
			_isRevealing = false;

			allRevealed.Invoke();
		}

		public void ShowNextParagraphWithoutAnimation()
		{
			if (IsAllRevealed()) return;

			StopAllCoroutines();

			var paragraphEnd = GetNextParagraphEnd(_nRevealedCharacters);
			_text.text = BuildPartiallyRevealedString(original: _originalString,
				keyCharIndex: paragraphEnd,
				minIndex: _nRevealedCharacters,
				maxIndex: paragraphEnd,
				fadeLength: 0);

			_nRevealedCharacters = paragraphEnd + 1;
			while (_nRevealedCharacters < _originalString.Length && _originalString[_nRevealedCharacters] == '\n')
				_nRevealedCharacters += 1;

			if (IsAllRevealed())
				allRevealed.Invoke();

			_isRevealing = false;
		}

		public void RevealNextParagraphAsync()
		{
			StartCoroutine(RevealNextParagraph());
		}

		public IEnumerator RevealNextParagraph()
		{
			if (IsAllRevealed() || _isRevealing) yield break;

			var paragraphEnd = GetNextParagraphEnd(_nRevealedCharacters);
			if (paragraphEnd < 0) yield break;

			_isRevealing = true;

			var keyChar = (float) (_nRevealedCharacters - numCharactersFade);
			var keyCharEnd = paragraphEnd;
			var speed = 0f;
			var secondsElapsed = 0f;

			while (keyChar < keyCharEnd)
			{
				secondsElapsed += Time.deltaTime;
				if (secondsElapsed <= smoothSeconds)
					speed = Mathf.Lerp(0f, charsPerSecond, secondsElapsed / smoothSeconds);
				else
				{
					var secondsLeft = (keyCharEnd - keyChar) / charsPerSecond;
					if (secondsLeft < smoothSeconds)
						speed = Mathf.Lerp(charsPerSecond, 0.1f * charsPerSecond, 1f - secondsLeft / smoothSeconds);
				}

				keyChar = Mathf.MoveTowards(keyChar, keyCharEnd, speed * Time.deltaTime);
				_text.text = BuildPartiallyRevealedString(original: _originalString,
					keyCharIndex: keyChar,
					minIndex: _nRevealedCharacters,
					maxIndex: paragraphEnd,
					fadeLength: numCharactersFade);

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

		private string BuildPartiallyRevealedString(string original, float keyCharIndex, int minIndex, int maxIndex,
			int fadeLength)
		{
			if (original.Length == 0)
			{
				return "";
			}

			var lastFullyVisibleChar = Mathf.Max(Mathf.CeilToInt(keyCharIndex), minIndex - 1);
			var firstFullyInvisibleChar = (int) Mathf.Min(keyCharIndex + fadeLength, maxIndex) + 1;

			var revealed = original.Substring(0, lastFullyVisibleChar + 1);
			var unrevealed = original.Substring(firstFullyInvisibleChar);

			var sb = new StringBuilder();
			sb.Append(revealed);

			for (var i = lastFullyVisibleChar + 1; i < firstFullyInvisibleChar; ++i)
			{
				var c = original[i];
				var originalColorRGB = ColorUtility.ToHtmlStringRGB(_text.color);
				var alpha = Mathf.RoundToInt(255 * (keyCharIndex - i) / fadeLength);
				sb.AppendFormat("<color=#{0}{1:X2}>{2}</color>", originalColorRGB, (byte) alpha, c);
			}

			sb.AppendFormat("<color=#00000000>{0}</color>", unrevealed);
			return sb.ToString();
		}

		private void Awake()
		{
			// Grab the text component
			_text = GetComponent <TextMeshProUGUI>();

			if (dialogueSettings != null) _text.font = dialogueSettings.font;
		}
	}
}
