using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Item Management/Item Collection Channel")]
	// ReSharper disable once InconsistentNaming
	public class CollectionEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.ScriptableObjects.InventorySO> OnInventoryEventRaised;
		public UnityAction<Components.ScriptableObjects.RecipeCollectionSO> OnRecipeEventRaised;

		/// <summary>
		/// Can be used to for example change the inventory.
		/// </summary>
		/// <param name="inventory"></param>
		public void RaiseEvent(Components.ScriptableObjects.InventorySO inventory) => OnInventoryEventRaised?.Invoke(inventory);

		/// <summary>
		/// Can be use to set different crafting/cooking shops.
		/// </summary>
		/// <param name="recipeCollection"></param>
		public void RaiseEvent(Components.ScriptableObjects.RecipeCollectionSO recipeCollection) => OnRecipeEventRaised?.Invoke(recipeCollection);
	}
}
