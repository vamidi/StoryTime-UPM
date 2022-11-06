using UnityEditor;
using UnityEditor.Callbacks;

using StoryTime.VisualScripting.Data.ScriptableObjects;
namespace StoryTime.Editor.VisualScripting.Utilities
{
	public static partial class AssetHandler
	{
		[OnOpenAsset]
		// Handles opening the editor window when double-clicking project files
		public static bool OnOpenAsset(int instanceID, int line)
		{
			DialogueContainerSO container = EditorUtility.InstanceIDToObject(instanceID) as DialogueContainerSO;
			if (container != null)
			{
				DialogueEditorWindow.OpenDialogueGraphWindow(container);
				return true;
			}

			return false;
		}
	}
}
