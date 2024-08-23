using UnityEngine;

namespace StoryTime.Domains.SceneManagement.SpawnSystems
{
	public abstract class BaseSpawn : MonoBehaviour
	{
		public bool IsStarted => m_Started;

		private bool m_Started = false;

		public virtual void Respawn()
		{
			m_Started = true;
		}

		protected virtual void Awake()
		{
            m_Started = false;
		}

		protected virtual void OnEnable()
		{
			m_Started = false;
		}
	}
}
