using System;
using UnityEngine;
using UnityEngine.Localization;

using StoryTime.Components.ScriptableObjects;
namespace StoryTime.Components
{
	[Serializable]
	public class Attribute
	{
		[Tooltip("Name of the attribute")] public LocalizedString attributeName = new ();
		[Tooltip("Alias for the attribute")] public AttributeType alias = AttributeType.HP;
	}
}
