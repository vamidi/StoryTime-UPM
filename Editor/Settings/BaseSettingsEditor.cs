using StoryTime.Configurations.ScriptableObjects;
using StoryTime.Editor.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StoryTime.Editor.Settings
{
	public abstract class BaseSettingsEditor<T>: UnityEditor.Editor
		where T : ScriptableObject
	{
		public override void OnInspectorGUI()
		{
			GUILayout.Space(10);
			if (GUILayout.Button("Open StoryTime Settings Window", GUILayout.Height(30)))
				StoryTimeSettingsProvider.Open();
			GUILayout.Space(10);

			if (GetSetting() == target)
			{
				EditorGUILayout.HelpBox("This asset contains the currently active settings for the Input System.", MessageType.Info);
			}
			else
			{
				string currentlyActiveAssetsPath = null;
				if (GetSetting() != null)
					currentlyActiveAssetsPath = AssetDatabase.GetAssetPath(GetSetting());
				if (!string.IsNullOrEmpty(currentlyActiveAssetsPath))
					currentlyActiveAssetsPath = $"The currently active settings are stored in {currentlyActiveAssetsPath}. ";

				EditorGUILayout.HelpBox($"Note that this asset does not contain the currently active settings for the Input System. {currentlyActiveAssetsPath??""}Click \"Make Active\" below to make {target.name} the active one.", MessageType.Warning);

				// TODO implement
				// if (GUILayout.Button($"Make active", EditorStyles.miniButton))
					// InputSystem.settings = (InputSettings)target;
			}
		}

		protected abstract T GetSetting();
	}
}
