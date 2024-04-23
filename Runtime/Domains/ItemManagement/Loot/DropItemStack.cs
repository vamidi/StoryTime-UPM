using System;

namespace StoryTime.Components
{
	/// <summary>
	/// Drop Item Change Class
	/// </summary>
	[Serializable]
	public class DropItemStack : DropStack<ScriptableObjects.ItemSO>
	{
		public DropItemStack() { }
		public DropItemStack(DropItemStack item) : base(item) { }
		public DropItemStack(ScriptableObjects.ItemSO item, int amount) : base(item, amount) { }

	}
}
