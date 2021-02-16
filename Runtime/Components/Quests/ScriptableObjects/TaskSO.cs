using UnityEngine;

namespace DatabaseSync.Components
{
	public enum TaskCompletionType
	{
		Collect,
		Defeat,
		Talk,
		Interact,
		Defend
	}

	[CreateAssetMenu(fileName = "Task", menuName = "DatabaseSync/Quests/Task", order = 51)]
	// ReSharper disable once InconsistentNaming
	public class TaskSO : ScriptableObject
	{
		[Tooltip("The Character this mission will need interaction with")]
		[SerializeField]
		private ActorSO actor;

		[Tooltip("The dialogue that will be displayed before an action, if any")]
		[SerializeField]
		private StorySO dialogueBeforeTask;

		[Tooltip("The dialogue that will be displayed when the step is achieved")]
		[SerializeField]
		private StorySO winDialogue;

		[Tooltip("The dialogue that will be diplayed if the step is not achieved yet")]
		[SerializeField]
		private StorySO loseDialogue;

		[Tooltip("The item to check/give/reward")]
		[SerializeField]
		private Item item;

		[Tooltip("The type of the task")]
		[SerializeField]
		private TaskCompletionType type;

		[SerializeField]
		bool isDone;

		public StorySO DialogueBeforeTask => dialogueBeforeTask;
		public StorySO WinDialogue => winDialogue;
		public StorySO LoseDialogue => loseDialogue;
		public Item Item => item;
		public TaskCompletionType Type => type;
		public bool IsDone => isDone;
		public ActorSO Actor => actor;

		public void FinishTask()
		{
			isDone = true;
		}
	}
}
