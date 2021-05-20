using UnityEngine;

namespace DatabaseSync.Components
{
	public abstract class EnemyBehaviour : MonoBehaviour
	{
		public PlayerController Target => m_Target;
		public EnemyController Controller => m_EnemyController;
		public Damageable Damageable => m_Damageable;

		// [Header("References")]
		protected PlayerController m_Target;
		protected EnemyController m_EnemyController;

		private Damageable m_Damageable;

		protected virtual void OnEnable()
		{
			m_Damageable = GetComponentInChildren<Damageable>();

			if(m_Damageable.DamageReceiverChannel != null)
				m_Damageable.DamageReceiverChannel.OnEventRaised += OnReceiveMessage;

			m_EnemyController = GetComponentInChildren<EnemyController>();
		}

		protected virtual void OnDisable()
		{
			if(m_Damageable.DamageReceiverChannel != null)
				m_Damageable.DamageReceiverChannel.OnEventRaised -= OnReceiveMessage;
		}

		public virtual void OnReceiveMessage(MessageType type, Damageable.DamageMessage msg) { }
	}
}
