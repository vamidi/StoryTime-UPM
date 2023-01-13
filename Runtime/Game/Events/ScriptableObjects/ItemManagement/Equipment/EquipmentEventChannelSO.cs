using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Item Management/Equipment Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class EquipmentEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.ScriptableObjects.CharacterSO, Components.ItemStack> OnEventRaised;

		public void RaiseEvent(Components.ScriptableObjects.CharacterSO character, Components.ItemStack equipment) => OnEventRaised?.Invoke(character, equipment);
	}
}
