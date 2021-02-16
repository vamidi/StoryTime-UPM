using System;
using UnityEngine;
using UnityEngine.Events;

namespace DatabaseSync
{
	[CreateAssetMenu(fileName = "newDialogueOptionEvent", menuName = "DatabaseSync/Events/Narrative/Dialogue Choice Event")]
	public class DialogueChoiceEventChannelSO : ScriptableObject
	{
		[Tooltip("Dialogue Option Event name")]
		[SerializeField] public string EventName = String.Empty;

		[Tooltip("Dialogue Option Event name")]
		[SerializeField] public uint Value = UInt32.MaxValue;

		// [Tooltip("Dialogue Option reference"), HideInInspector]
		// public DialogueChoiceSO DialogueOption;

		// [Tooltip("Actor reference"), HideInInspector]
		// public ActorSO actor;

		[Tooltip("Dialogue Option Event value")]
		public UnityAction<string, UnityEngine.Object> OnEventRaised;

		public void RaiseEvent(UnityEngine.Object value)
		{
			if (OnEventRaised != null)
				OnEventRaised.Invoke(EventName, value);
		}
	}
}
