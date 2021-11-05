using UnityEditor;
using UnityEngine;

namespace StoryTime.Editor.Input
{
	[CustomEditor(typeof(StoryTime.Input.ScriptableObjects.BaseInputReader))]
	public class InputReaderEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (!Application.isPlaying)
				return;

			ScriptableObjectHelper.GenerateButtonsForEvents<StoryTime.Input.ScriptableObjects.BaseInputReader>(target);
		}
	}
}
