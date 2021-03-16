using UnityEngine;
using UnityEditor;

using TMPro.EditorUtilities;

namespace DatabaseSync.Editor.UI
{
	using Components;

	[CustomEditor(typeof(TMPVertexAnimator), true)]
	[CanEditMultipleObjects]
	public class TMPAnimatedEditor : TMP_BaseEditorPanel
	{
		private SerializedProperty m_ActionProp;
		private SerializedProperty m_EmotionProp;
		private SerializedProperty m_CharRevealProp;
		private SerializedProperty m_AllRevealedProp;

		private SerializedProperty m_AudioSourceGroupProp;

		private SerializedProperty m_UseConfigProp;

		private SerializedProperty m_CharPerSecondProp;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_ActionProp = serializedObject.FindProperty("onAction");
			m_EmotionProp = serializedObject.FindProperty("onEmotionChange");
			m_CharRevealProp = serializedObject.FindProperty("onCharReveal");
			m_AllRevealedProp = serializedObject.FindProperty("allRevealed");

			m_UseConfigProp = serializedObject.FindProperty("useConfig");

			m_CharPerSecondProp = serializedObject.FindProperty("charsPerSecond");

			m_AudioSourceGroupProp = serializedObject.FindProperty("audioSourceGroup");
		}

		protected override void OnUndoRedo()
		{
		}

		protected override void DrawExtraSettings()
		{
			EditorGUILayout.LabelField("Event Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_ActionProp);
			EditorGUILayout.PropertyField(m_EmotionProp);
			EditorGUILayout.PropertyField(m_CharRevealProp);
			EditorGUILayout.PropertyField(m_AllRevealedProp);

			EditorGUILayout.LabelField("Dialogue Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_UseConfigProp);
			EditorGUILayout.PropertyField(m_CharPerSecondProp, new GUIContent("     Default Character Speed"));

			EditorGUILayout.LabelField("Audio Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_AudioSourceGroupProp);
		}

		protected override bool IsMixSelectionTypes()
		{
			return false;
		}
	}
}
