using UnityEngine.Events;
using UnityEngine;

namespace DatabaseSync
{
	/// <summary>
	/// This class is used for talk interaction events.
	/// Example: start talking to an actor passed as parameter
	/// </summary>

	[CreateAssetMenu(menuName = "DatabaseSync/Events/Narrative/Dialogue Event Channel")]
	public class DialogueActorChannelSO : ScriptableObject
	{
		public UnityAction<Components.ActorSO> OnEventRaised;

		public void RaiseEvent(Components.ActorSO actor)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(actor);
		}
	}
}
