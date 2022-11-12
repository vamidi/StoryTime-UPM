using UnityEditor;
using UnityEditor.Callbacks;

using StoryTime.Components.ScriptableObjects;
namespace StoryTime.Editor.VisualScripting.Utilities
{
	public static partial class AssetHandler
	{
		[OnOpenAsset]
		// Handles opening the editor window when double-clicking project files
		public static bool OnOpenAsset(int instanceID, int line)
		{
			StorySO container = EditorUtility.InstanceIDToObject(instanceID) as StorySO;
			if (container != null)
			{
				DialogueEditorWindow.OpenDialogueGraphWindow(container);
				return true;
			}

			return false;
		}
	}
}
