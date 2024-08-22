using UnityEngine;

namespace StoryTime.Domains.Narrative.Dialogues
{
	using StoryTime.Domains.Narrative.Stories.ScriptableObjects;
	
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
