using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync.Events
{
	public enum TaskEventType
	{
		CurrentCharacterRevision
	}

	[CreateAssetMenu(menuName = "DatabaseSync/Events/Stories/Task Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class TaskEventChannelSO : ScriptableObject
	{
		public UnityAction<Components.TaskSO> OnEventRaised;


		/// <summary>
		///
		/// </summary>
		/// <param name="task"></param>
		public void RaiseEvent(Components.TaskSO task)
		{
			OnEventRaised?.Invoke(task);
		}
	}
}
