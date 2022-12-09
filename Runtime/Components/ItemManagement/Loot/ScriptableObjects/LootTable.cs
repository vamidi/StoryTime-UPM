using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "Item", menuName = "StoryTime/Game/Item Management/Loot table", order = 55)]
	public class LootTable : ScriptableObject
	{
		public List<DropItemStack> guaranteedLootTable = new ();
		public List<DropItemStack> oneItemFromList =  new (1);

		public float weightToNoDrop = 100;

		/// <summary>
		///
		/// </summary>
		/// <param name="_position"></param>
		/// <param name="_count"></param>
		/// <param name="_range"></param>
		public void SpawnDrop(Transform _position, int _count, float _range)
		{
			List<GameObject> guaranteed = GetGuaranteedLoot();
			List<GameObject> randomLoot = GetRandomLoot(_count);

			for (int i = 0; i < guaranteed.Count; i++)
			{
				Instantiate(guaranteed[i], new Vector3(_position.position.x + Random.Range(-_range, _range), _position.position.y, _position.position.z + Random.Range(-_range, _range)), Quaternion.identity);
			}

			for (int i = 0; i < randomLoot.Count; i++)
			{
				Instantiate(randomLoot[i], new Vector3(_position.position.x + Random.Range(-_range, _range), _position.position.y, _position.position.z + Random.Range(-_range, _range)), Quaternion.identity);
			}
		}

		/// <summary>
		/// Return List of Guaranteed Drop
		/// </summary>
		/// <returns></returns>
		private List<GameObject> GetGuaranteedLoot()
		{
			List<GameObject> lootList = new List<GameObject>();

			foreach (var guaranteed in guaranteedLootTable)
			{
				// Adds the drawn number of items to drop
				int count = Random.Range(guaranteed.MinCountItem, guaranteed.MaxCountItem);
				for (int j = 0; j < count; j++)
				{
					lootList.Add(guaranteed.Item.Prefab);
				}
			}

			return lootList;
		}

		/// <summary>
		/// Return List of Optional Drop
		/// </summary>
		/// <param name="ChangeCount"></param>
		/// <returns></returns>
		private List<GameObject> GetRandomLoot(int ChangeCount)
		{
			List<GameObject> lootList = new List<GameObject>();
			float totalWeight = weightToNoDrop;

			// Executes a function a specified number of times
			for (int j = 0; j < ChangeCount; j++)
			{
				// They add up the entire weight of the items
				for (int i = 0; i < oneItemFromList.Count; i++)
				{
					totalWeight += oneItemFromList[i].Weight;
				}

				float value = Random.Range(0, totalWeight);
				float timed_value = 0;

				for (int i = 0; i < oneItemFromList.Count; i++)
				{
					// If timed_value is greater than value, it means this item has been drawn
					timed_value += oneItemFromList[i].Weight;
					if (timed_value > value)
					{
						int count = Random.Range(oneItemFromList[i].MinCountItem, oneItemFromList[i].MaxCountItem + 1);
						for (int c = 0; c < count; c++)
						{
							lootList.Add(oneItemFromList[i].Item.Prefab);
						}
						break;
					}
				}
			}

			return lootList;
		}
	}
}
