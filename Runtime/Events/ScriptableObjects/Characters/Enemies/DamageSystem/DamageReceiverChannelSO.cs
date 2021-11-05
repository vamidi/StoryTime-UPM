using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	[CreateAssetMenu(menuName = "StoryTime/Events/Characters/Enemy Damage Receiver", order = 51)]
	public class DamageReceiverChannelSO : ScriptableObject
	{
		public UnityAction<Components.MessageType, Components.Damageable.DamageMessage> OnEventRaised;

		public void RaiseEvent(Components.MessageType type, Components.Damageable.DamageMessage dmgMessage) => OnEventRaised?.Invoke(type, dmgMessage);
	}
}
