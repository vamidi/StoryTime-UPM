using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.ItemManagement.Crafting.ScriptableObjects;

using StoryTime.Domains.Events.ScriptableObjects;
namespace StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Item Management/Item Collection Channel")]
	// ReSharper disable once InconsistentNaming
	public class CollectionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<InventorySO> OnInventoryEventRaised;
		public UnityAction<RecipeCollectionSO> OnRecipeEventRaised;

		/// <summary>
		/// Can be used to for example change the inventory.
		/// </summary>
		/// <param name="inventory"></param>
		public void RaiseEvent(InventorySO inventory) => OnInventoryEventRaised?.Invoke(inventory);

		/// <summary>
		/// Can be use to set different crafting/cooking shops.
		/// </summary>
		/// <param name="recipeCollection"></param>
		public void RaiseEvent(RecipeCollectionSO recipeCollection) => OnRecipeEventRaised?.Invoke(recipeCollection);
	}
}
