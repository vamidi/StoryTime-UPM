using System;
using UnityEngine;

namespace DatabaseSync.Components
{
	using Binary;

	// [CreateAssetMenu(fileName = "newDialogueEvent", menuName = "DatabaseSync/Events/Narrative/Dialogue Event")]
	[Serializable]
	// ReSharper disable once InconsistentNaming
	public class DialogueEventSO /*: ScriptableObject */
	{
		public string EventName => eventName;

		public UnityEngine.Object Value => value;

		[Tooltip("Dialogue Option Event name")]
		[SerializeField] private string eventName;

		[Tooltip("Dialogue Option Event value you want to pass")]
		[SerializeField] private UnityEngine.Object value;

		public DialogueEventSO(string eventName, UnityEngine.Object value)
		{
			this.eventName = eventName;
			this.value = value;
		}
		public override string ToString()
		{
			return $"Event {eventName} and value {value}";
		}
	}
}
