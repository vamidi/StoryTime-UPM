using UnityEngine;
using UnityEngine.UIElements;


namespace StoryTime.Editor.Domains.UI.Editors
{
	using StoryTime.Editor.Domains.Wizards;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;

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
