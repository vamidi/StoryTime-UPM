using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
namespace StoryTime.Domains.Game.Characters.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Characters/Character Event Channel")]
	public class CharacterEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<CharacterSO> OnEventRaised;

		public void RaiseEvent(CharacterSO character) => OnEventRaised?.Invoke(character);
	}
}
