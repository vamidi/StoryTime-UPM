using System;
using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Game.Characters
{
	using ScriptableObjects;

	[Serializable]
	public class Attribute
	{
		[Tooltip("Name of the attribute")] public LocalizedString attributeName = new ();
		[Tooltip("Alias for the attribute")] public AttributeType alias = AttributeType.HP;
	}
}
