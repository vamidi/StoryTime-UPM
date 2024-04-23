using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Item", menuName = "StoryTime/Game/Item Management/Loot table", order = 55)]
	public class LootTable : BaseLootTable<DropItemStack, ItemSO>
	{
		protected override void GetLoot(out List<GameObject> lootList)
		{
			lootList = new();
			foreach (var loot in GetGuaranteedLoot())
			{
				lootList.Add(loot.Item.Prefab);
			}
		}

		protected override List<GameObject> GetRandomLoot(List<DropItemStack> lootList)
		{
			List<GameObject> gameObjectLootList = new();
			foreach (var loot in lootList)
			{
				gameObjectLootList.Add(loot.Item.Prefab);

			}

			return gameObjectLootList;
		}
	}
}
