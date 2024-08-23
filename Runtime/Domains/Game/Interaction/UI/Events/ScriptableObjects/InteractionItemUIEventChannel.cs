using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Events.ScriptableObjects.UI
{
	using StoryTime.Domains.Game.Interaction;
	using StoryTime.Domains.ItemManagement.Inventory;

	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Toggle Interaction Item UI Event Channel")]
	public class InteractionItemUIEventChannel : EventChannelBaseSO
	{
		public UnityAction<bool, ItemStack, InteractionType> OnEventRaised;
		public UnityAction<bool, List<ItemStack>, InteractionType> OnEventsRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="state"></param>
		/// <param name="itemInfo"></param>
		/// <param name="interactionType"></param>
		public void RaiseEvent(bool state, ItemStack itemInfo, InteractionType interactionType)
			=> OnEventRaised?.Invoke(state, itemInfo, interactionType);

		public void RaiseEvent(bool state, List<ItemStack> itemInfo, InteractionType interactionType)
			=> OnEventsRaised?.Invoke(state, itemInfo, interactionType);
	}
}
