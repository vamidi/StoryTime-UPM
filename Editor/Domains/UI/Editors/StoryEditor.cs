using UnityEngine.UIElements;


namespace StoryTime.Editor.Domains.UI.Editors
{
	using StoryTime.Editor.Domains.Wizards;
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
	public class StoryEditor : EditorTab<CharacterWizard, StorySO>
	{
		internal new class UxmlFactory : UxmlFactory<StoryEditor> {}

		public StoryEditor()
		{
			var asset = UIResourceHelper.GetTemplateAsset($"Editors/{nameof(StoryEditor)}");
			asset.CloneTree(this);
		}

		protected override void DrawSelection(Box cardInfo, StorySO enumerable)
		{

		}
	}
}
