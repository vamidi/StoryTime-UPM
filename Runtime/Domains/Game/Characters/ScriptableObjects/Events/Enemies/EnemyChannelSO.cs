using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Game.Characters.ScriptableObjects.Events.Enemies
{
	using StoryTime.Domains.Game.NPC.Enemies.ScriptableObjects;
	
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Enemy Channel")]
	public class EnemyChannelSO : ScriptableObject
	{
		public UnityAction<EnemySO> OnEventRaised;
		public void RaiseEvent(EnemySO enemySo)
		{
			OnEventRaised?.Invoke(enemySo);
		}
	}
}
