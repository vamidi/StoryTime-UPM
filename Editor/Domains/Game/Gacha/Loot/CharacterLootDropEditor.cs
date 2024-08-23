
using UnityEditor;

namespace StoryTime.Editor.Domains.Game.Gacha.Loot
{
	using StoryTime.Domains.Game.Gacha.Loot;
	using StoryTime.Editor.Domains.ItemManagement.Loot;
	using StoryTime.Domains.Game.Characters.ScriptableObjects;
	using StoryTime.Domains.Game.Gacha.Loot.ScriptableObjects;
	
	[CustomEditor(typeof(CharacterLootTable))]
	public class CharacterLootDropEditor : BaseLootDropEditor<CharacterLootTable, DropCharacterStack, CharacterSO>
	{
		protected override bool VerifyItem(CharacterSO item)
		{
			return item != null;
		}

		protected override string GetLootName(CharacterSO item)
		{
			return item.name;
		}
	}
}
