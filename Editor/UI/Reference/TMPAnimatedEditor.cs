using UnityEngine;
using UnityEditor;

using TMPro.EditorUtilities;

namespace DatabaseSync.UI
{
	using Components;

	[CustomEditor(typeof(TMPAnimated), true)]
	[CanEditMultipleObjects]
	public class TMPAnimatedEditor : TMP_BaseEditorPanel
	{
		private SerializedProperty m_ActionProp;
		private SerializedProperty m_SpeedProp;
		private SerializedProperty m_EmotionProp;
		private SerializedProperty m_PunctuationSourceProp;
		private SerializedProperty m_VoiceSourceProp;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_ActionProp = serializedObject.FindProperty("onAction");
			m_SpeedProp = serializedObject.FindProperty("speed");
			m_PunctuationSourceProp = serializedObject.FindProperty("punctuationSource");
			m_VoiceSourceProp = serializedObject.FindProperty("voiceSource");
			m_EmotionProp = serializedObject.FindProperty("onEmotionChange");
		}

		protected override void OnUndoRedo()
		{
		}

		protected override void DrawExtraSettings()
		{
			EditorGUILayout.LabelField("Action Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_ActionProp);

			EditorGUILayout.LabelField("Audio Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_PunctuationSourceProp);
			EditorGUILayout.PropertyField(m_VoiceSourceProp);

			EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_SpeedProp, new GUIContent("     Default Speed"));
			EditorGUILayout.PropertyField(m_EmotionProp);
		}

		protected override bool IsMixSelectionTypes()
		{
			return false;
		}
	}
}
