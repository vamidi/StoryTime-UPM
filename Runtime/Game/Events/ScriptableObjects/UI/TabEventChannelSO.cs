using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/UI/Inventory Tab Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class TabEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.ScriptableObjects.InventoryTabTypeSO> OnEventRaised;
		public void RaiseEvent(Components.ScriptableObjects.InventoryTabTypeSO item) => OnEventRaised?.Invoke(item);
	}
}
