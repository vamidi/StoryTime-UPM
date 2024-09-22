using UnityEngine;

namespace StoryTime.Domains.Narrative.Dialogues
{
	using StoryTime.Domains.Narrative.Stories;
	
	public class DialogueTrigger : MonoBehaviour
	{
		[SerializeField] private DialogueManager dialogueManager;
		[SerializeField] private IReadOnlyStory storyData = default;

		private void OnTriggerEnter(Collider other)
		{
			dialogueManager.Interact(storyData);
		}
	}
}
