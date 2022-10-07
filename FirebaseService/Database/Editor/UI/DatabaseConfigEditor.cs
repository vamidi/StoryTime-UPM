using UnityEngine;

namespace StoryTime.Editor.UI
{
	[UnityEditor.CustomEditor(typeof(Configurations.ScriptableObjects.FirebaseConfigSO), true)]
	public class DatabaseConfigEditor : UnityEditor.Editor
	{
		static readonly GUIContent EditConfig = new("Open", "Open config file in the Database settings window.");

		public override void OnInspectorGUI()
		{
			if (GUILayout.Button(EditConfig))
			{
				DatabaseSyncWindow.OpenWindow();
			}
		}
	}
}
