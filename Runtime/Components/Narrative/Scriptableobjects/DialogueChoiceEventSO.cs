using System;
using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "newDialogueChoiceEvent", menuName = "StoryTime/Game/Events/Narrative/Dialogue Choice Event")]
	public class DialogueChoiceEventSO : ScriptableObject
	{
		public string EventName => eventName;

		public ScriptableObject Value => value;

		[Tooltip("Dialogue Option Event name")]
		[SerializeField] private string eventName = String.Empty;

		[Tooltip("Dialogue Option Event value you want to pass")]
		[SerializeField] private ScriptableObject value;
	}
}
