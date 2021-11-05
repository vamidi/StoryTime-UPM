using UnityEngine;
using UnityEngine.Events;

namespace StoryTime.Events.ScriptableObjects
{

	/// <summary>
	/// This class is used for Events that have no arguments (Example: Exit game event)
	/// </summary>

	[CreateAssetMenu(menuName = "StoryTime/Events/Void Event Channel")]
	public class VoidEventChannelSO : EventChannelBaseSO
	{
		public UnityAction OnEventRaised;

		public void RaiseEvent() => OnEventRaised?.Invoke();
	}
}


