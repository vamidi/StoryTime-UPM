using UnityEngine;

namespace StoryTime.Domains.SceneManagement
{
	public class LocationEntrance : MonoBehaviour
	{
		[Header("Asset References")]
		[SerializeField] private ScriptableObjects.PathSO entrancePath;

		public ScriptableObjects.PathSO EntrancePath => entrancePath;
	}
}
