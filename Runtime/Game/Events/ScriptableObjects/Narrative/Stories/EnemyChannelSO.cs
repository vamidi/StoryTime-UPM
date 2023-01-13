using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Enemy Channel")]
	public class EnemyChannelSO : ScriptableObject
	{
		public UnityAction<Components.ScriptableObjects.EnemySO> OnEventRaised;
		public void RaiseEvent(Components.ScriptableObjects.EnemySO enemySo)
		{
			OnEventRaised?.Invoke(enemySo);
		}
	}
}
