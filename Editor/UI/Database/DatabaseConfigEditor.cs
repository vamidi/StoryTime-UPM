using UnityEngine;

namespace DatabaseSync.Editor.UI
{
	[UnityEditor.CustomEditor(typeof(DatabaseConfig), true)]
	public class DatabaseConfigEditor : UnityEditor.Editor
	{
		static readonly GUIContent EditConfig = new GUIContent("Change", "Open config file in the Database settings window.");

		public override void OnInspectorGUI()
		{
			if (GUILayout.Button(EditConfig))
			{
			}
		}
	}
}
