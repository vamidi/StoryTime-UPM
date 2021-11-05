using System;
using StoryTime.Binary;

public abstract class BaseTable<T> : UnityEditor.MonoScript where T : StoryTime.Components.ScriptableObjects.TableBehaviour
{
	public static UnityEditor.MonoScript ConvertRow(TableRow row, T scriptableObject = null)
	{
		throw new ArgumentException("Row can't be converted. Make a new class that inherits from this class");
	}
}
