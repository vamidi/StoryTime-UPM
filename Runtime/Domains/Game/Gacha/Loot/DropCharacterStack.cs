using System;

using StoryTime.Domains.Game.Characters.ScriptableObjects;
using StoryTime.Domains.ItemManagement;

namespace StoryTime.Domains.Game.Gacha.Loot
{
	[Serializable]
	public class DropCharacterStack : DropStack<CharacterSO>
	{
		public DropCharacterStack() { }
		public DropCharacterStack(DropCharacterStack item) : base(item) { }
		public DropCharacterStack(CharacterSO item, int amount) : base(item, amount) { }
	}
}
