using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;

	public class LootSpawner : MonoBehaviour
	{
		public LootTable loot;
		public int randomDropCount = 1;
		public float dropRange = .5f;

		protected void SpawnLoot()
		{
			loot.SpawnDrop(this.transform, randomDropCount, dropRange);
		}
	}
}
