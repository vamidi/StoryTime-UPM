using UnityEngine;

namespace DatabaseSync.Components
{
	[CreateAssetMenu(menuName = "DatabaseSync/Events/Stories/Task Event")]
	public class TaskEventSO : ScriptableObject
	{
		public CharacterSO Character => character;
		public Events.TaskEventType TaskEventType => taskEventType;
		public ScriptableObject Value => value;

		[Tooltip("Actor reference")]
		[SerializeField] private CharacterSO character;

		[Tooltip("Specify the type of the event")]
		[SerializeField] private Events.TaskEventType taskEventType;

		[Tooltip("Value we want to pass")]
		[SerializeField] private ScriptableObject value;
	}
}
