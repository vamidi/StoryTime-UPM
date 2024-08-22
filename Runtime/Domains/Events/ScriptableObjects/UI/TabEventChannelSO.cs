using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.ItemManagement.Inventory.ScriptableObjects;
namespace StoryTime.Domains.Events.ScriptableObjects.UI
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/UI/Inventory Tab Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class TabEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<InventoryTabTypeSO> OnEventRaised;
		public void RaiseEvent(InventoryTabTypeSO item) => OnEventRaised?.Invoke(item);
	}
}
