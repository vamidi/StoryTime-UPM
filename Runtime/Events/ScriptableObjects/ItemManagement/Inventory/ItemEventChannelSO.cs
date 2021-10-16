using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Item Management/Item Event Channel")]
	public class ItemEventChannelSO : EventChannelBaseSO
	{
		[Header("Items")]
		public UnityAction<Components.ItemStack> OnItemEventRaised;
		public UnityAction<List<Components.ItemStack>> OnItemsEventRaised;

		[Header("Recipes")]
		public UnityAction<Components.ItemRecipeStack> OnRecipeEventRaised;
		public UnityAction<List<Components.ItemRecipeStack>> OnRecipesEventRaised;

		public void RaiseEvent(Components.ItemStack item) => OnItemEventRaised?.Invoke(item);

		public void RaiseEvent(List<Components.ItemStack> items) => OnItemsEventRaised?.Invoke(items);

		public void RaiseEvent(Components.ItemRecipeStack item) => OnRecipeEventRaised?.Invoke(item);

		public void RaiseEvent(List<Components.ItemRecipeStack> items) => OnRecipesEventRaised?.Invoke(items);
	}
}
