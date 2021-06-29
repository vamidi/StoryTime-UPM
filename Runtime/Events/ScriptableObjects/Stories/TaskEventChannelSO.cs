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
	public class TaskEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.TaskSO, Components.TaskEventSO> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="task"></param>
		/// <param name="value"></param>
		public void RaiseEvent(Components.TaskSO task, Components.TaskEventSO value)
		{
			OnEventRaised?.Invoke(task, value);
		}
	}
}
