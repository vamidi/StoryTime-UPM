using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/UI/Dialogue line Channel")]
	public class DialogueLineChannelSO : ScriptableObject
	{
		public UnityAction<Components.DialogueLineSO, Components.ActorSO> OnEventRaised;

		public void RaiseEvent(Components.DialogueLineSO line, Components.ActorSO actor)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(line, actor);
		}
	}
}
