using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StoryTime.Domains.Utilities.Types
{
	[Serializable]
	public class CollectionWrapperBase {}

	/// <summary>
	/// CollectionWrapper used to apply custom drawers to Array fields
	/// </summary>
	[Serializable]
	public class CollectionWrapper<T> : CollectionWrapperBase
	{
		// ReSharper disable once InconsistentNaming
		public List<T> Value;
	}

	[CustomPropertyDrawer(typeof(CollectionWrapperBase), true)]
	public class CollectionWrapperDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var collection = property.FindPropertyRelative("Value");
			return EditorGUI.GetPropertyHeight(collection, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var collection = property.FindPropertyRelative("Value");
			EditorGUI.PropertyField(position, collection, label, true);
		}
	}
}
