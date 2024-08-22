using UnityEngine;

using StoryTime.Domains.Events.ScriptableObjects;
using StoryTime.Domains.Game.Characters.ScriptableObjects;
namespace StoryTime.Domains.Narrative.Tasks.ScriptableObjects.Events
{
	/// <summary>
	/// Task event in Scriptable object form.
	/// You can override this object and add values to it.
	/// </summary>
	[CreateAssetMenu(menuName = "StoryTime/Game/Events/Narrative/Task Event")]
	// ReSharper disable once InconsistentNaming
	public class TaskEventSO : EventChannelBaseSO
	{
		public CharacterSO Character => character;
		public TaskEventType TaskEventType => taskEventType;

		[Tooltip("Actor reference")]
		[SerializeField] private CharacterSO character;

		[Tooltip("Specify the type of the event")]
		[SerializeField] private TaskEventType taskEventType;
	}
}
