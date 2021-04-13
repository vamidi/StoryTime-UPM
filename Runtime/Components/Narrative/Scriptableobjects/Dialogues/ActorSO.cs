using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync.Components
{
	/// <summary>
	/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
	/// </summary>

	[CreateAssetMenu(fileName = "newActor", menuName = "DatabaseSync/Stories/Actor")]
	// ReSharper disable once InconsistentNaming
	public partial class ActorSO : TableBehaviour
	{
		public LocalizedString ActorName => actorName;

		[SerializeField] private LocalizedString actorName = default;

		public ActorSO() : base("characters", "name") { }
	}
}
