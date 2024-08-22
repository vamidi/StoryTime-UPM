using UnityEngine;
using UnityEngine.Events;

using StoryTime.Domains.Events.ScriptableObjects;
namespace StoryTime.Domains.Narrative.Tasks.ScriptableObjects.Events
{
	public enum TaskEventType
	{
		CurrentCharacterRevision
	}

	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Task Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class TaskEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<TaskSO, TaskEventSO> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="task"></param>
		/// <param name="value"></param>
		public void RaiseEvent(TaskSO task, TaskEventSO value)
		{
			OnEventRaised?.Invoke(task, value);
		}
	}
}
