using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Components.ScriptableObjects
{
	[CreateAssetMenu(fileName = "GachaSystem", menuName = "StoryTime/Game/Characters/Gacha System", order = 55)]
	public class CharacterLootTable : BaseLootTable<DropCharacterStack, CharacterSO>
	{
		public void Spawn(int count)
		{
			List<DropCharacterStack> characters = GetRandom(count);
		}
	}
}
