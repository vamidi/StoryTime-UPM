using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/UI/Inventory Tab Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class TabEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<InventoryTabTypeSO> OnEventRaised;
		public void RaiseEvent(InventoryTabTypeSO item) => OnEventRaised?.Invoke(item);
	}
}
