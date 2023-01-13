using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{
	public enum TaskEventType
	{
		CurrentCharacterRevision
	}

	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Task Event Channel")]
	// ReSharper disable once InconsistentNaming
	public class TaskEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Components.ScriptableObjects.TaskSO, Components.ScriptableObjects.TaskEventSO> OnEventRaised;

		/// <summary>
		///
		/// </summary>
		/// <param name="task"></param>
		/// <param name="value"></param>
		public void RaiseEvent(Components.ScriptableObjects.TaskSO task, Components.ScriptableObjects.TaskEventSO value)
		{
			OnEventRaised?.Invoke(task, value);
		}
	}
}
