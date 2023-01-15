using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;

	public class GachaSpawner : MonoBehaviour
	{
		[SerializeField] private CharacterLootTable loot;
		[SerializeField, Range(1, 100), Tooltip("Amount of time we roll for an item.")] private int randomDropCount = 1;

		protected void Spawn()
		{
			loot.Spawn(randomDropCount);
		}
	}
}
