using System;

namespace StoryTime.Domains.Database
{
	using StoryTime.Domains.Database.Binary;
	using StoryTime.Domains.Database.ScriptableObjects;

	public abstract class BaseTableHandler<T> : UnityEditor.MonoScript
		where T : TableBehaviour
	{
		public static UnityEditor.MonoScript ConvertRow(TableRow row, T scriptableObject = null)
		{
			throw new ArgumentException("Row can't be converted. Make a new class that inherits from this class");
		}
	}
}
