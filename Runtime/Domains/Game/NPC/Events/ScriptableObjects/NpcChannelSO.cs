using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Game.NPC.Events.ScriptableObjects
{
	using StoryTime.Domains.Game.NPC.ScriptableObjects;

	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/NPC Channel")]
	public class NpcChannelSO : ScriptableObject
	{
		public UnityAction<NonPlayableActorSO> OnEventRaised;
		public void RaiseEvent(NonPlayableActorSO nonPlayableActorSo)
		{
			OnEventRaised?.Invoke(nonPlayableActorSo);
		}
	}
}
