using UnityEngine;


namespace StoryTime.Domains.ItemManagement.Inventory.UI
{
	using ScriptableObjects;
	
	public class ItemManager<TStack, TItem, TCollection> : MonoBehaviour
		where TItem: ItemSO
		where TStack: BaseStack<TItem>, new()
		where TCollection: ItemCollection<TStack, TItem>
	{
		[SerializeField] protected TCollection currentInventory;
	}
}
