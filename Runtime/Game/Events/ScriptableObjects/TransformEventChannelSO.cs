using UnityEngine.Events;
using UnityEngine;

namespace StoryTime.Events.ScriptableObjects
{
	/// <summary>
	/// This class is used for Events that have one transform argument.
	/// Example: Spawn system initializes player and fire event, where the transform is the position of player.
	/// </summary>
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Transform Event Channel")]
	public class TransformEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<Transform> OnEventRaised;

		public void RaiseEvent(Transform value)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(value);
		}
	}
}
