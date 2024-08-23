using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Domains.Game.NPC.Events.ScriptableObjects
{
	using StoryTime.Domains.Game.DamageSystem;

	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Characters/Enemy Damage Receiver", order = 51)]
	public class DamageReceiverChannelSO : ScriptableObject
	{
		public UnityAction<MessageType, Damageable.DamageMessage> OnEventRaised;

		public void RaiseEvent(MessageType type, Damageable.DamageMessage dmgMessage) => OnEventRaised?.Invoke(type, dmgMessage);
	}
}
