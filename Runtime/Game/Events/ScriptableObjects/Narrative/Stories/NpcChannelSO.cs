using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/NPC Channel")]
	public class NpcChannelSO : ScriptableObject
	{
		public UnityAction<Components.ScriptableObjects.NonPlayableActorSO> OnEventRaised;
		public void RaiseEvent(Components.ScriptableObjects.NonPlayableActorSO nonPlayableActorSo)
		{
			OnEventRaised?.Invoke(nonPlayableActorSo);
		}
	}
}
