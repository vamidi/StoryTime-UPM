using UnityEngine;

namespace StoryTime.Components
{
	public class CollectibleItem : MonoBehaviour
	{
		[SerializeField] private ItemStack currentItem;

		public ItemStack GetItem()
		{
			return currentItem;
		}

		public void SetItem(ScriptableObjects.ItemSO item, int amount)
		{
			currentItem = new ItemStack(item, amount);
		}
	}
}
