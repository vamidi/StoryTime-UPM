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
			var story = EditorUtility.InstanceIDToObject(instanceID) as StorySO;
			if (story != null)
			{
				DialogueGraphEditorWindow.OpenDialogueGraphWindow(story);
				return true;
			}

			var recipe = EditorUtility.InstanceIDToObject(instanceID) as ItemRecipeSO;
			if (recipe != null)
			{
				ItemGraphEditorWindow.OpenRecipeGraphWindow(recipe);
				return true;
			}

			return false;
		}
	}
}
