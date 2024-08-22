using UnityEditor;

using StoryTime.Components;
using StoryTime.Editor.Game.Components;
using StoryTime.Components.ScriptableObjects;
using StoryTime.Domains.Game.Characters.ScriptableObjects;

// ReSharper disable once CheckNamespace
namespace StoryTime.Editor.Components
{
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
