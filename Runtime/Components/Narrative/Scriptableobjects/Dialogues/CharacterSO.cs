using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync.Components
{
	/// <summary>
	/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
	/// </summary>

	[CreateAssetMenu(fileName = "newCharatcer", menuName = "DatabaseSync/Stories/Character")]
	// ReSharper disable once InconsistentNaming
	public partial class CharacterSO : TableBehaviour
	{
		public LocalizedString CharacterName => characterName;

		[SerializeField] private LocalizedString characterName = default;

		public CharacterSO() : base("characters", "name") { }
	}
}
