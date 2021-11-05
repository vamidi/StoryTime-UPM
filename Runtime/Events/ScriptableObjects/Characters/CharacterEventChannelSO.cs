using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Characters/Character Event Channel")]
	public class CharacterEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.ScriptableObjects.CharacterSO> OnEventRaised;

		public void RaiseEvent(Components.ScriptableObjects.CharacterSO character) => OnEventRaised?.Invoke(character);
	}
}
