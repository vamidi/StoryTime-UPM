using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Characters/Character Event Channel")]
	public class CharacterEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.CharacterSO> OnEventRaised;

		public void RaiseEvent(Components.CharacterSO character) => OnEventRaised?.Invoke(character);
	}
}
