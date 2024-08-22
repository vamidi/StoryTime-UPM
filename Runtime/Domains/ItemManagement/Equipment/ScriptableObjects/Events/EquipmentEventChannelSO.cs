using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
using StoryTime.Domains.Game.Characters.ScriptableObjects;

namespace StoryTime.Domains.ItemManagement.Equipment.ScriptableObjects.Events
{
	using Inventory;
	
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Item Management/Equipment Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class EquipmentEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<CharacterSO, ItemStack> OnEventRaised;

		public void RaiseEvent(CharacterSO character, ItemStack equipment) => OnEventRaised?.Invoke(character, equipment);
	}
}
