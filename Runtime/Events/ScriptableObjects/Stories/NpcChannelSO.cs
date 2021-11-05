using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Stories/NPC Channel")]
	public class NpcChannelSO : ScriptableObject
	{
		public UnityAction<Components.ScriptableObjects.NonPlayableActorSO> OnEventRaised;
		public void RaiseEvent(Components.ScriptableObjects.NonPlayableActorSO nonPlayableActorSo)
		{
			OnEventRaised?.Invoke(nonPlayableActorSo);
		}
	}
}
