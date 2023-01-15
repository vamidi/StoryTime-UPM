using System;

namespace StoryTime.Components
{
	[Serializable]
	public class DropCharacterStack : DropStack<ScriptableObjects.CharacterSO>
	{
		public DropCharacterStack() { }
		public DropCharacterStack(DropCharacterStack item) : base(item) { }
		public DropCharacterStack(ScriptableObjects.CharacterSO item, int amount) : base(item, amount) { }
	}
}
