using UnityEditor;
using UnityEngine;

namespace DatabaseSync.Editor.Input
{
	using DatabaseSync.Input;

	[CustomEditor(typeof(BaseInputReader))]
	public class InputReaderEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (!Application.isPlaying)
				return;

			ScriptableObjectHelper.GenerateButtonsForEvents<BaseInputReader>(target);
		}
	}
}
