using UnityEngine;

namespace StoryTime.Domains.Game.NPC.ScriptableObjects
{
	using StoryTime.Domains.Database.ScriptableObjects;
	
	[CreateAssetMenu(fileName = "newNpc", menuName = "StoryTime/Game/NPC")]
	public class NonPlayableActorSO : TableBehaviour
	{
		public string Npc => _npc;

		[SerializeField] private string _npc = default;

		public NonPlayableActorSO() : base("nonPlayableCharacters", "name") { }
	}
}
