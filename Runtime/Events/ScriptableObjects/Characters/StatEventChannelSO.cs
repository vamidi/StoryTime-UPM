using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Characters/Stat Event Channel")]
	public class StatEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Game.CharacterStats> OnEventRaised;

		public void RaiseEvent(Game.CharacterStats stats) => OnEventRaised?.Invoke(stats);

	}
}
