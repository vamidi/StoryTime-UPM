using UnityEngine;

namespace StoryTime.Components
{
	using StoryTime.Domains.ItemManagement.Loot.ScriptableObjects;

	public class LootSpawner : MonoBehaviour
	{
		/// <summary>
		/// Loot manager
		/// </summary>
		[SerializeField] private LootTable loot;
		/// <summary>
		/// How often we random drop an item.
		/// </summary>
		[SerializeField, Range(1, 100), Tooltip("Amount of time we roll for an item.")] private int randomDropCount = 1;
		[SerializeField, Tooltip("The range of the drop of an item.")] private float dropRange = .5f;

		protected void SpawnLoot()
		{
			loot.SpawnDrop(this.transform, randomDropCount, dropRange);
		}
	}
}
