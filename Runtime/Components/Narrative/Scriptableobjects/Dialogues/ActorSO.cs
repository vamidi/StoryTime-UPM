using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync.Components
{
	/// <summary>
	/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
	/// </summary>

	[CreateAssetMenu(fileName = "newActor", menuName = "DatabaseSync/Narrative/Actor")]
	// ReSharper disable once InconsistentNaming
	public class ActorSO : TableBehaviour
	{
		public LocalizedString ActorName
		{
			get => actorName;
			set => actorName = value;
		}

		[SerializeField] private LocalizedString actorName = default;

		public ActorSO() : base("characters", "name") { }
	}
}
