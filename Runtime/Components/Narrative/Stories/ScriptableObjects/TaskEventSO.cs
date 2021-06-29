using UnityEngine;

namespace DatabaseSync.Components
{
	/// <summary>
	/// Task event in Scriptable object form.
	/// You can override this object and add values to it.
	/// </summary>
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Stories/Task Event")]
	// ReSharper disable once InconsistentNaming
	public class TaskEventSO : ScriptableObject
	{
		public CharacterSO Character => character;
		public Events.TaskEventType TaskEventType => taskEventType;

		[Tooltip("Actor reference")]
		[SerializeField] private CharacterSO character;

		[Tooltip("Specify the type of the event")]
		[SerializeField] private Events.TaskEventType taskEventType;
	}
}
