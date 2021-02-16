using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Task Channel")]
	// ReSharper disable once InconsistentNaming
	public class TaskChannelSO : ScriptableObject
	{
		public UnityAction<Components.TaskSO> OnEventRaised;
		public void RaiseEvent(Components.TaskSO task)
		{
			OnEventRaised?.Invoke(task);
		}
	}
}
