using System;
using StoryTime.Binary;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

public abstract class BaseTable<T> : UnityEditor.MonoScript where T : StoryTime.Components.ScriptableObjects.TableBehaviour
{
	public static UnityEditor.MonoScript ConvertRow(TableRow row, T scriptableObject = null)
	{
		throw new ArgumentException("Row can't be converted. Make a new class that inherits from this class");
	}
}
