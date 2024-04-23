using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;
	using Events.ScriptableObjects;

	public class EquipmentManager : MonoBehaviour
	{
		// We have a character
		[SerializeField] private CharacterSO character;

		[Header("Listening on channels")]
		[SerializeField] private EquipmentEventChannelSO equipItemEvent;

		private void OnEnable()
		{
			equipItemEvent.OnEventRaised += EquipItemEventRaised;
		}

		private void Start()
		{
			if (character && character.CharacterClass)
			{
				foreach (var characterEquipment in character.Equipments)
				{
					EquipItem(characterEquipment);
				}
			}
		}

		private void EquipItemEventRaised(CharacterSO selectedCharacter, ItemStack equipment)
		{
			if (character == selectedCharacter)
				EquipItem(equipment.Item as EquipmentSO);
		}

		private void EquipItem(EquipmentSO equipment)
		{
			if(equipment)
				character.Equip(equipment);
		}
	}
}
