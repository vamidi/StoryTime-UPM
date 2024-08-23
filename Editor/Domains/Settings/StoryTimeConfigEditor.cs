using System;
using UnityEditor;

using UnityEngine;

namespace StoryTime.Editor.Domains.Settings
{
	using StoryTime.Editor.Domains.UI.Providers;
	
	// [CustomEditor(typeof(StoryTimeSettingsSO))]
	public class StoryTimeConfigEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			GUILayout.Space(10);
			if (GUILayout.Button("Open StoryTime Settings Window", GUILayout.Height(30)))
				StoryTimeSettingsProvider.Open();
			GUILayout.Space(10);
		}
	}
}
