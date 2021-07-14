using System.Collections.Generic;

using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class StoryEditor : EditorTab<Components.StorySO>
	{
		internal new class UxmlFactory : UxmlFactory<StoryEditor> {}

		public StoryEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(StoryEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
		}

		protected override void DrawSelection(Box cardInfo, Components.StorySO enumerable)
		{

		}
	}
}
