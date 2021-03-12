using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using TMPro;

namespace DatabaseSync.Components
{
	using Binary;

	[Serializable] public class CharRevealEvent : UnityEvent<char> { }

	public class TMPVertexAnimator : TextMeshProUGUI
	{
		public struct TextAnimInfo
		{
			public Vector2Int Positions;
			public TextAnimationType Type;
		}

		public Events.StringEventChannelSO onAction;
		public Events.EmotionEventChannelSO onEmotionChange;
		public CharRevealEvent onCharReveal;

		public AudioSourceGroup audioSourceGroup;

		public bool useConfig = true;

		public int numCharactersFade = 3;
		public float charsPerSecond = 1 / 30f;
		public float smoothSeconds = 0.75f;

		public UnityEvent allRevealed = new UnityEvent();
		public bool IsRevealing => _isRevealing;

		private static readonly Color32 Clear = new Color32(0, 0, 0, 0);

		private float m_TimeUntilNextDialogueSound;
		private float m_LastDialogueSound;

		private const float CHAR_ANIM_TIME = 0.07f;
		private const float CHAR_FADE_SPEED = 1f; // is equal 100%
		private const float NOISE_MAGNITUDE_ADJUSTMENT = 0.06f;
		private const float NOISE_FREQUENCY_ADJUSTMENT = 15f;
		private const float WAVE_MAGNITUDE_ADJUSTMENT = 0.06f;

		private string m_OriginalString = String.Empty;
		private string m_ProcessedMessage = String.Empty;

		private int _nRevealedCharacters;
		private bool m_StopAnimating;
		private bool _isRevealing;

		private DialogueSettingConfig m_Config;

		private List<DialogueCommand> m_Commands = new List<DialogueCommand>();

		// cache the color for when the player skips the text
		private TMP_MeshInfo[] m_CachedMeshInfo;
		private Color32[][] m_OriginalColors;

		private Coroutine m_RevealRoutine;

		public bool IsAllRevealed()
		{
			return _nRevealedCharacters >= m_OriginalString.Length;
		}

		public void RestartWithText(string strText)
		{
			m_OriginalString = strText;
			_nRevealedCharacters = 0;
			_isRevealing = false;

			// Strip all the text from tags.
			m_Commands.Clear();
			text = m_ProcessedMessage;
		}

		public void ShowEverythingWithoutAnimation()
		{
			StopAllCoroutines();

			// TODO call all actions and emotions
			foreach (var command in m_Commands)
			{
				EvaluateTag(command);
			}

			text = m_ProcessedMessage;
			_nRevealedCharacters = m_ProcessedMessage.Length;

			TextAnimInfo[] textAnimInfo = SeparateOutTextAnimInfo(m_Commands);

			for (int i = 0; i < textInfo.characterCount; i++)
			{
				TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
				int vertexIndex = charInfo.vertexIndex;
				int materialIndex = charInfo.materialReferenceIndex;

				Color32[] destinationColors = textInfo.meshInfo[materialIndex].colors32;
				Color32 theColor = m_OriginalColors[materialIndex][vertexIndex];
				theColor.a = 255;

				ShowCharacterColor(theColor, vertexIndex, ref destinationColors);
				ShowCharacterSize(m_CachedMeshInfo, textAnimInfo, materialIndex, i, vertexIndex, 1);
			}

			UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			for (int i = 0; i < textInfo.meshInfo.Length; i++) {
				TMP_MeshInfo theInfo = textInfo.meshInfo[i];
				theInfo.mesh.vertices = theInfo.vertices;
				UpdateGeometry(theInfo.mesh, i);
			}

			_isRevealing = false;

			allRevealed.Invoke();
		}

		public void ShowNextParagraphWithoutAnimation()
		{
			if (IsAllRevealed()) return;

			// StopAllCoroutines();

			m_StopAnimating = true;

			// StartCoroutine(RevealNextParagraph());
		}

		public void RevealNextParagraphAsync()
		{
			m_Commands = DialogueUtility.ProcessInputString(m_OriginalString, out m_ProcessedMessage);

			if (m_Config != null)
			{
				if (!m_Config.AnimatedText) // show character for character without animation
				{
					ShowNextParagraphWithoutAnimation();
					return;
				}

				if (m_Config.ShowDialogueAtOnce) // Show all dialogue at once
				{
					ShowEverythingWithoutAnimation();
					return;
				}
			}

			// StartCoroutine(RevealNextParagraph());
			if(m_RevealRoutine != null) StopCoroutine(m_RevealRoutine);
			m_RevealRoutine = StartCoroutine(RevealNextParagraph(m_Commands));
		}

		public IEnumerator RevealNextParagraph(List<DialogueCommand> commands)
		{
			_isRevealing = true;

			// float secondsPerCharacter = 1f / 10f;
			float secondsPerCharacter = charsPerSecond;
			float timeOfLastCharacter = 0;

			TextAnimInfo[] textAnimInfo = SeparateOutTextAnimInfo(commands);

			foreach (TMP_MeshInfo meshInfo in textInfo.meshInfo)
			{
				if (meshInfo.vertices != null) {
					for (int j = 0; j < meshInfo.vertices.Length; j++) {
						meshInfo.vertices[j] = Vector3.zero;
					}
				}
			}

			text = m_ProcessedMessage;
			ForceMeshUpdate();

			m_CachedMeshInfo = textInfo.CopyMeshInfoVertexData();
			m_OriginalColors = new Color32[textInfo.meshInfo.Length][];
			for (int i = 0; i < m_OriginalColors.Length; i++) {
				Color32[] theColors = textInfo.meshInfo[i].colors32;
				m_OriginalColors[i] = new Color32[theColors.Length];
				Array.Copy(theColors, m_OriginalColors[i], theColors.Length);
			}

			// The amount of characters in the processed message.
			int charCount = textInfo.characterCount;
			float[] charAnimStartTimes = new float[charCount];
			for (int i = 0; i < charCount; i++) {
				charAnimStartTimes[i] = -1; // indicate the character as not yet started animating.
			}
			int visibleCharacterIndex = 0;
			while (true)
			{
				// while (_nRevealedCharacters < _originalString.Length && _originalString[_nRevealedCharacters] == '\n')
					// _nRevealedCharacters += 1;

				if (m_StopAnimating) {
	                for (int i = visibleCharacterIndex; i < charCount; i++)
	                {
	                    charAnimStartTimes[i] = Time.unscaledTime;
	                }
	                visibleCharacterIndex = charCount;
	                FinishAnimating();
	            }
	            if (ShouldShowNextCharacter(secondsPerCharacter, timeOfLastCharacter)) {
	                if (_nRevealedCharacters <= charCount) {
		                ExecuteCommandsForCurrentIndex(commands, visibleCharacterIndex, ref secondsPerCharacter, ref timeOfLastCharacter);
		                if (visibleCharacterIndex < charCount && ShouldShowNextCharacter(secondsPerCharacter, timeOfLastCharacter)) {
	                        charAnimStartTimes[visibleCharacterIndex] = Time.unscaledTime;
	                        onCharReveal.Invoke(m_ProcessedMessage[visibleCharacterIndex]);
	                        visibleCharacterIndex++;
	                        timeOfLastCharacter = Time.unscaledTime;
	                        if (visibleCharacterIndex == charCount) {
	                            FinishAnimating();
	                        }
	                    }
	                }
	            }

				for (int j = 0; j < charCount; j++)
				{
	                TMP_CharacterInfo charInfo = textInfo.characterInfo[j];
	                if (charInfo.isVisible) //Invisible characters have a vertexIndex of 0 because they have no vertices and so they should be ignored to avoid messing up the first character in the string whic also has a vertexIndex of 0
	                {
		                int vertexIndex = charInfo.vertexIndex;
	                    int materialIndex = charInfo.materialReferenceIndex;
	                    Color32[] destinationColors = textInfo.meshInfo[materialIndex].colors32;
	                    Color32 theColor = j < visibleCharacterIndex ? m_OriginalColors[materialIndex][vertexIndex] : Clear;

	                    float charAnimStartTime = charAnimStartTimes[j];
	                    float charSize = 0;
	                    if (charAnimStartTime >= 0) {
	                        float timeSinceAnimStart = Time.unscaledTime - charAnimStartTime;
	                        theColor.a = (byte)Mathf.Min(255, 255 * CHAR_FADE_SPEED * timeSinceAnimStart);
	                        charSize = Mathf.Min(1, timeSinceAnimStart / CHAR_ANIM_TIME);
	                    }

	                    ShowCharacterColor(theColor, vertexIndex, ref destinationColors);
		                ShowCharacterSize(m_CachedMeshInfo, textAnimInfo, materialIndex, j, vertexIndex, charSize);
	                }
	            }

	            UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
	            for (int i = 0; i < textInfo.meshInfo.Length; i++) {
	                TMP_MeshInfo theInfo = textInfo.meshInfo[i];
	                theInfo.mesh.vertices = theInfo.vertices;
	                UpdateGeometry(theInfo.mesh, i);
	            }
	            yield return null;
	        }
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

			onCharReveal.AddListener(OnCharacterRevealed);
		}

		private static bool ShouldShowNextCharacter(float secondsPerCharacter, float timeOfLastCharacter) {
			return (Time.unscaledTime - timeOfLastCharacter) > secondsPerCharacter;
		}

		private void ShowCharacterColor(Color32 theColor, int vertexIndex, ref Color32[] destinationColors)
		{
			destinationColors[vertexIndex + 0] = theColor;
            destinationColors[vertexIndex + 1] = theColor;
            destinationColors[vertexIndex + 2] = theColor;
            destinationColors[vertexIndex + 3] = theColor;
		}

		private void ShowCharacterSize(TMP_MeshInfo[] cachedMeshInfo, TextAnimInfo[] textAnimInfo, int materialIndex, int charIndex, int vertexIndex, float charSize)
		{
			Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
			Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

			Vector3 animPosAdjustment = GetAnimPosAdjustment(textAnimInfo, charIndex, fontSize, Time.unscaledTime);
			Vector3 offset = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

			destinationVertices[vertexIndex + 0] = ((sourceVertices[vertexIndex + 0] - offset) * charSize) + offset + animPosAdjustment * 2;
			destinationVertices[vertexIndex + 1] = ((sourceVertices[vertexIndex + 1] - offset) * charSize) + offset + animPosAdjustment;
			destinationVertices[vertexIndex + 2] = ((sourceVertices[vertexIndex + 2] - offset) * charSize) + offset + animPosAdjustment * 2;
			destinationVertices[vertexIndex + 3] = ((sourceVertices[vertexIndex + 3] - offset) * charSize) + offset + animPosAdjustment;
		}

		private void EvaluateTag(DialogueCommand command)
		{
			switch (command.Type)
			{
				case DialogueCommandType.Emotion:
					if(onEmotionChange != null) onEmotionChange.RaiseEvent(command.TextEmotionValue);
					break;
				case DialogueCommandType.Action:
					if(onAction != null) onAction.RaiseEvent(command.StringValue);
					break;
			}
		}

		private void OnCharacterRevealed(char c)
		{
			if (m_Config == null)
				return;

			/*
			if (char.IsPunctuation(c) && m_Config.PunctuationClip && !punctuationSource.isPlaying)
			{
				voiceSource.Stop();
				punctuationSource.clip = m_Config.PunctuationClip;
				punctuationSource.Play();
			}
			*/

			if (char.IsLetter(c) && m_Config.VoiceClip)
			{
				if (Time.unscaledTime - m_LastDialogueSound > m_TimeUntilNextDialogueSound) {
					m_TimeUntilNextDialogueSound = UnityEngine.Random.Range(0.02f, 0.08f);
					m_LastDialogueSound = Time.unscaledTime;
					audioSourceGroup.PlayFromNextSource(m_Config.VoiceClip); // Use Multiple Audio Sources to allow playing multiple sounds at once
				}

				// mouthQuad.localScale = new Vector3(1, 0, 1);
				// mouthQuad.DOScaleY(1, .2f).OnComplete(() => mouthQuad.DOScaleY(0, .2f));
			}
		}

		private void FinishAnimating()
		{
			m_StopAnimating = false;

			if (IsAllRevealed())
				allRevealed.Invoke();

			_isRevealing = false;
		}

		private Vector3 GetAnimPosAdjustment(TextAnimInfo[] textAnimInfo, int charIndex, float curFontSize, float time) {
			float x = 0;
			float y = 0;
			foreach (var info in textAnimInfo)
			{
				if (charIndex >= info.Positions.x && charIndex < info.Positions.y)
				{
					if (info.Type == TextAnimationType.Shake)
					{
						float scaleAdjust = curFontSize * NOISE_MAGNITUDE_ADJUSTMENT;
						x += (Mathf.PerlinNoise((charIndex + time) * NOISE_FREQUENCY_ADJUSTMENT, 0) - 0.5f) * scaleAdjust;
						y += (Mathf.PerlinNoise((charIndex + time) * NOISE_FREQUENCY_ADJUSTMENT, 1000) - 0.5f) * scaleAdjust;
					} else if (info.Type == TextAnimationType.Wave)
					{
						y += Mathf.Sin((charIndex * 1.5f) + (time * 6)) * curFontSize * WAVE_MAGNITUDE_ADJUSTMENT;
					}
				}
			}
			return new Vector3(x, y, 0);
		}

		private void ExecuteCommandsForCurrentIndex(List<DialogueCommand> commands, int visibleCharacterIndex, ref float secondsPerCharacter, ref float timeOfLastCharacter)
		{
			for (int i = 0; i < commands.Count; i++)
			{
				DialogueCommand command = commands[i];
				if (command.Position == visibleCharacterIndex)
				{
					switch (command.Type)
					{
						case DialogueCommandType.Pause:
							timeOfLastCharacter = Time.unscaledTime + command.FloatValue;
							break;
						case DialogueCommandType.TextSpeedChange:
							secondsPerCharacter = 1f / command.FloatValue;
							break;
						case DialogueCommandType.Action:
						case DialogueCommandType.Emotion:
							EvaluateTag(command);
							break;
					}
					commands.RemoveAt(i);
					i--;
				}
			}
		}

		private TextAnimInfo[] SeparateOutTextAnimInfo(List<DialogueCommand> commands)
		{
			var animCommands = commands.Where(c => c.Type == DialogueCommandType.AnimStart || c.Type == DialogueCommandType.AnimEnd).ToArray();
			commands.RemoveAll(c =>  c.Type == DialogueCommandType.AnimStart || c.Type == DialogueCommandType.AnimEnd);

			Debug.Assert(animCommands.Length % 2 == 0, "Unequal number of start and end animation commands.");

			List<TextAnimInfo> tempResult = new List<TextAnimInfo>();
			for (int i = 0; i < animCommands.Length; i += 2)
			{
				var startCommand = animCommands[i];
				tempResult.Add(new TextAnimInfo
				{
					Positions = new Vector2Int(startCommand.Position, animCommands[i+1].Position),
					Type = startCommand.TextAnimValue
				});
			}

			return tempResult.ToArray();
		}
	}
}
