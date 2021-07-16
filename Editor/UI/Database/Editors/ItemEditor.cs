using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace DatabaseSync.Editor.UI
{
	public class ItemEditor : EditorTab<Components.ItemSO>
	{
		internal new class UxmlFactory : UxmlFactory<ItemEditor> {}

		public ItemEditor()
		{
			var asset = Resources.GetTemplateAsset($"Editors/{nameof(ItemEditor)}");
			asset.CloneTree(this);

			var listView = this.Q<ListView>("item-list");
			Initialize(listView);
		}

		protected override void DrawSelection(Box cardInfo, Components.ItemSO item)
		{
			if (item.PreviewImage == null)
				return;

			try
			{
				LoadPreviewImage(item.PreviewImage.texture);
			}
			catch (UnassignedReferenceException)
			{
			}
		}
	}
}
