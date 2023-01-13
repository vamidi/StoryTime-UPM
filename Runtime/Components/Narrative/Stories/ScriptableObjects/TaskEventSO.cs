using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	/// <summary>
	/// Task event in Scriptable object form.
	/// You can override this object and add values to it.
	/// </summary>
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Task Event")]
	// ReSharper disable once InconsistentNaming
	public class TaskEventSO : ScriptableObject
	{
		public CharacterSO Character => character;
		public Events.ScriptableObjects.TaskEventType TaskEventType => taskEventType;

		[Tooltip("Actor reference")]
		[SerializeField] private CharacterSO character;

		[Tooltip("Specify the type of the event")]
		[SerializeField] private Events.ScriptableObjects.TaskEventType taskEventType;
	}
}
