using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Item Management/Equipment Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class EquipmentEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.CharacterSO, Components.ItemStack> OnEventRaised;

		public void RaiseEvent(Components.CharacterSO character, Components.ItemStack equipment) => OnEventRaised?.Invoke(character, equipment);
	}
}
