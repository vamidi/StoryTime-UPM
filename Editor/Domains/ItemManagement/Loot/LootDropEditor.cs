using UnityEditor;

namespace StoryTime.Editor.Domains.ItemManagement.Loot
{
	using StoryTime.Domains.ItemManagement.Loot;
	using StoryTime.Domains.ItemManagement.Loot.ScriptableObjects;
	using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
	
	[CustomEditor(typeof(LootTable))]
	public class LootDropEditor : BaseLootDropEditor<LootTable, DropItemStack, ItemSO>
	{
		protected override bool VerifyItem(ItemSO item)
		{
			return item != null && item.Prefab != null;
		}

		protected override string GetLootName(ItemSO item)
		{
			return item.Prefab.name;
		}

		protected override void ValidateGuaranteedList(LootTable loot)
		{
			base.ValidateGuaranteedList(loot);
			bool _prefabError = false;

			foreach (var guaranteed in loot.guaranteedLootTable)
			{
				if (guaranteed.Item != null && guaranteed.Item.Prefab == null) { _prefabError = true; }
			}
			if (_prefabError == true) { EditorGUILayout.HelpBox("One of the List Items does not have ''Item To Drop'' assigned, which will cause an error if it is drawn", MessageType.Error, true); }
		}

		protected override void ValidateOneItemFromList(LootTable loot)
		{
			base.ValidateOneItemFromList(loot);
			bool _prefabError = false;

			foreach (var oneItem in loot.oneItemFromList)
			{
				if (oneItem.Item != null && oneItem.Item.Prefab == null) { _prefabError = true; }
			}
			if (_prefabError == true) { EditorGUILayout.HelpBox("One of the List Items does not have ''Item To Drop'' assigned, which will cause an error if it is drawn", MessageType.Error, true); }

		}
	}
}
