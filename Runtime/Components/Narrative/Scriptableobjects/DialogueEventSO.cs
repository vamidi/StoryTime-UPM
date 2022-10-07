using System;
using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	using FirebaseService.Database.Binary;

	// [CreateAssetMenu(fileName = "newDialogueEvent", menuName = "StoryTime/Events/Narrative/Dialogue Event")]
	[Serializable]
	// ReSharper disable once InconsistentNaming
	public class DialogueEventSO /*: ScriptableObject */
	{
		public string EventName => eventName;

		public dynamic Value => value;

		[Tooltip("Dialogue Option Event name")]
		[SerializeField] private string eventName;

		// Dialogue Option Event value you want to pass
		// Extend class for more values.
		protected dynamic value;

		// Extend constructor to have more values
		public DialogueEventSO(string eventName, dynamic value = null)
		{
			this.eventName = eventName;
			this.value = value;
		}
		public override string ToString()
		{
			string displayedValue = Convert.ToString(value);
			return $"Event {eventName} and value {displayedValue}";
		}
	}
}
