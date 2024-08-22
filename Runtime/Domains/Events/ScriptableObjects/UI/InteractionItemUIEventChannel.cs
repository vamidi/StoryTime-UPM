using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.ItemManagement.Inventory;
namespace StoryTime.Domains.Events.ScriptableObjects.UI
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Toggle Interaction Item UI Event Channel")]
	public class InteractionItemUIEventChannel : EventChannelBaseSO
	{
		public UnityAction<bool, ItemStack, Components.InteractionType> OnEventRaised;
		public UnityAction<bool, List<ItemStack>, Components.InteractionType> OnEventsRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="state"></param>
		/// <param name="itemInfo"></param>
		/// <param name="interactionType"></param>
		public void RaiseEvent(bool state, ItemStack itemInfo, Components.InteractionType interactionType)
			=> OnEventRaised?.Invoke(state, itemInfo, interactionType);

		public void RaiseEvent(bool state, List<ItemStack> itemInfo, Components.InteractionType interactionType)
			=> OnEventsRaised?.Invoke(state, itemInfo, interactionType);
	}
}
