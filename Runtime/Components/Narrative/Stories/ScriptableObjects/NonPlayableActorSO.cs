using UnityEngine;

namespace DatabaseSync.Components
{
	[CreateAssetMenu(fileName = "newNpc", menuName = "DatabaseSync/Stories/NPC")]
	public class NonPlayableActorSO : TableBehaviour
	{
		public string Npc => _npc;

		[SerializeField] private string _npc = default;

		public NonPlayableActorSO() : base("nonPlayableCharacters", "name") { }
	}
}
