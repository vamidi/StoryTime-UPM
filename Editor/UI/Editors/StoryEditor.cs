using UnityEngine.UIElements;

using StoryTime.Editor.Wizards;
using StoryTime.Components.ScriptableObjects;
// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.UI
{
	public class StoryEditor : EditorTab<CharacterWizard, StorySO>
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
