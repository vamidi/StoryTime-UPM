using UnityEngine.UIElements;


using StoryTime.Components.ScriptableObjects;
namespace StoryTime.Editor.UI
{
	public class StoryEditor : EditorTab<StorySO>
	{
		internal new class UxmlFactory : UxmlFactory<StoryEditor> {}

		public StoryEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(StoryEditor)}");
			asset.CloneTree(this);
		}

		protected override void DrawSelection(Box cardInfo, StorySO enumerable)
		{

		}
	}
}
