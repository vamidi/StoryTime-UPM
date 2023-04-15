using UnityEngine;

using StoryTime.Database.ScriptableObjects;
namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "newNpc", menuName = "StoryTime/Game/NPC")]
	public class NonPlayableActorSO : TableBehaviour
	{
		public string Npc => _npc;

		[SerializeField] private string _npc = default;

		public NonPlayableActorSO() : base("nonPlayableCharacters", "name") { }
	}
}
