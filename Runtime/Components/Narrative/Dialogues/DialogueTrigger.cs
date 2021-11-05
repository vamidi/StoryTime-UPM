using UnityEngine;

namespace StoryTime.Components
{
	using ScriptableObjects;
	public class DialogueTrigger : MonoBehaviour
	{
		[SerializeField] private DialogueManager dialogueManager;
		[SerializeField] private SimpleStorySO storyData = default;

		private void OnTriggerEnter(Collider other)
		{
			dialogueManager.Interact(storyData);
		}
	}
}
