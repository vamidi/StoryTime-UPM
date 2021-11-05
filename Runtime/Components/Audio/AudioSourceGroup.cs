using UnityEngine;

namespace StoryTime.Components
{
	public class AudioSourceGroup : MonoBehaviour
	{
		[SerializeField] private AudioSource[] typingSources;

		private int m_NextTypeSource;

		public void PlayFromNextSource(AudioClip clip) {
			AudioSource nextSource = typingSources[m_NextTypeSource];
			nextSource.clip = clip;
			nextSource.Play();
			m_NextTypeSource = (m_NextTypeSource + 1) % typingSources.Length;
		}
	}
}
