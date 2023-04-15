using System;

using StoryTime.Database.Binary;
public abstract class BaseTableHandler<T> : UnityEditor.MonoScript where T : StoryTime.Database.ScriptableObjects.TableBehaviour
{
	public static UnityEditor.MonoScript ConvertRow(TableRow row, T scriptableObject = null)
	{
		throw new ArgumentException("Row can't be converted. Make a new class that inherits from this class");
	}
}
