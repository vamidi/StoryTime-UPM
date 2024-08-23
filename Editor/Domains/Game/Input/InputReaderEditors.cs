using UnityEditor;
using UnityEngine;

namespace StoryTime.Editor.Domains.Game.Input
{
	using StoryTime.Domains.Game.Input.ScriptableObjects;
	
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
