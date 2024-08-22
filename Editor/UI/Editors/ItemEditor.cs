using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.Editor.Wizards;
using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
using StoryTime.Editor.Domains.UI;

namespace StoryTime.Editor.UI
{
	public class ItemEditor : EditorTab<ItemWizard, ItemSO>
	{
		internal new class UxmlFactory : UxmlFactory<ItemEditor> {}

		public ItemEditor()
		{
			var asset = UIResourceHelper.GetTemplateAsset($"Editors/{nameof(ItemEditor)}");
			asset.CloneTree(this);
		}

		protected override void DrawSelection(Box cardInfo, ItemSO item)
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
