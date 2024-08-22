using System.Collections.Generic;

using UnityEngine;

namespace StoryTime.Domains.ItemManagement
{
	public abstract class BaseLootTable<T, TS> : ScriptableObject
		where T : DropStack<TS>
		where TS : ScriptableObject
	{
		public List<T> guaranteedLootTable = new ();
		public List<T> oneItemFromList =  new (1);

		public float weightToNoDrop = 100;

		/// <summary>
		///
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="count"></param>
		/// <param name="range"></param>
		public void SpawnDrop(Transform transform, int count, float range)
		{
			GetLoot(out var guaranteed);
			List<GameObject> randomLoot = GetRandomLoot(GetRandom(count));
			Vector3 position = transform.position;

			foreach (var t in guaranteed)
			{
				Instantiate(t, new Vector3(position.x + Random.Range(-range, range), position.y, position.z + Random.Range(-range, range)), Quaternion.identity);
			}

			foreach (var t in randomLoot)
			{
				Instantiate(t, new Vector3(position.x + Random.Range(-range, range), position.y, position.z + Random.Range(-range, range)), Quaternion.identity);
			}
		}

		/// <summary>
		/// Return List of Optional Drop
		/// </summary>
		/// <param name="changeCount"></param>
		/// <returns></returns>
		protected List<T> GetRandom(int changeCount)
		{
			List<T> lootList = new List<T>();
			float totalWeight = weightToNoDrop;

			// Executes a function a specified number of times
			for (int j = 0; j < changeCount; j++)
			{
				// They add up the entire weight of the items
				for (int i = 0; i < oneItemFromList.Count; i++)
				{
					totalWeight += oneItemFromList[i].Weight;
				}

				float value = Random.Range(0, totalWeight);
				float timedValue = 0;

				foreach (var item in oneItemFromList)
				{
					// If timed_value is greater than value, it means this item has been drawn
					timedValue += item.Weight;
					if (timedValue > value)
					{
						int count = Random.Range(item.MinCountItem, item.MaxCountItem + 1);
						for (int c = 0; c < count; c++)
						{
							lootList.Add(item);
						}
						break;
					}
				}
			}

			return lootList;
		}

		/// <summary>
		/// Return List of Guaranteed Drop
		/// </summary>
		/// <returns></returns>
		protected List<T> GetGuaranteedLoot()
		{
			List<T> lootList = new List<T>();

			foreach (var guaranteed in guaranteedLootTable)
			{
				// Adds the drawn number of items to drop
				int count = Random.Range(guaranteed.MinCountItem, guaranteed.MaxCountItem);
				for (int j = 0; j < count; j++)
				{
					lootList.Add(guaranteed);
				}
			}

			return lootList;
		}

		protected virtual void GetLoot(out List<GameObject> lootList)
		{
			throw new System.NotImplementedException();
		}

		protected virtual List<GameObject> GetRandomLoot(List<T> lootList)
		{
			throw new System.NotImplementedException();
		}
	}
}
