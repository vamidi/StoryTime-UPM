using UnityEngine;

namespace StoryTime.Components
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
			m_EnemyController = GetComponentInChildren<EnemyController>();
		}

		public virtual void OnReceiveMessage(MessageType type, object sender, object msg)
		{ }
	}
}
