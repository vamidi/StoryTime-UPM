using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
namespace StoryTime.Domains.Game.Characters.ScriptableObjects.Events
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Characters/Stat Event Channel")]
	public class StatEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<CharacterStats> OnEventRaised;

		public void RaiseEvent(CharacterStats stats) => OnEventRaised?.Invoke(stats);

	}
}
