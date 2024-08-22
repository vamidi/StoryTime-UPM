using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;

namespace StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects.Events
{
	using Crafting;

	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Item Management/Item Event Channel")]
	public class ItemEventChannelSO : EventChannelBaseSO
	{
		[Header("Items")]
		public UnityAction<ItemStack> OnItemEventRaised;
		public UnityAction<List<ItemStack>> OnItemsEventRaised;

		[Header("Recipes")]
		public UnityAction<ItemRecipeStack> OnRecipeEventRaised;
		public UnityAction<List<ItemRecipeStack>> OnRecipesEventRaised;

		public void RaiseEvent(ItemStack item) => OnItemEventRaised?.Invoke(item);

		public void RaiseEvent(List<ItemStack> items) => OnItemsEventRaised?.Invoke(items);

		public void RaiseEvent(ItemRecipeStack item) => OnRecipeEventRaised?.Invoke(item);

		public void RaiseEvent(List<ItemRecipeStack> items) => OnRecipesEventRaised?.Invoke(items);
	}
}
