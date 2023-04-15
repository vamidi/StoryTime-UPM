﻿using UnityEngine;
using UnityEngine.UIElements;

using StoryTime.Components.ScriptableObjects;

namespace StoryTime.Editor.UI
{
	public class ItemEditor : EditorTab<ItemSO>
	{
		internal new class UxmlFactory : UxmlFactory<ItemEditor> {}

		public ItemEditor()
		{
			var asset = StoryTime.Editor.UI.Resources.GetTemplateAsset($"Editors/{nameof(ItemEditor)}");
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