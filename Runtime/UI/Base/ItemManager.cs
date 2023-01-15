using UnityEngine;

using StoryTime.Components.ScriptableObjects;
namespace StoryTime.Components.UI
{
	public class ItemManager<TStack, TItem, TCollection> : MonoBehaviour
		where TItem: ItemSO
		where TStack: BaseStack<TItem>, new()
		where TCollection: ItemCollection<TStack, TItem>
	{
		[SerializeField] protected TCollection currentInventory;
	}
}
