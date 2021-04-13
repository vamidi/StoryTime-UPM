using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Item Management/Item Collection Channel")]
	// ReSharper disable once InconsistentNaming
	public class CollectionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.InventorySO> OnInventoryEventRaised;
		public UnityAction<Components.RecipeCollectionSO> OnRecipeEventRaised;

		/// <summary>
		/// Can be used to for example change the inventory.
		/// </summary>
		/// <param name="inventory"></param>
		public void RaiseEvent(Components.InventorySO inventory) => OnInventoryEventRaised?.Invoke(inventory);

		/// <summary>
		/// Can be use to set different crafting/cooking shops.
		/// </summary>
		/// <param name="recipeCollection"></param>
		public void RaiseEvent(Components.RecipeCollectionSO recipeCollection) => OnRecipeEventRaised?.Invoke(recipeCollection);
	}
}
