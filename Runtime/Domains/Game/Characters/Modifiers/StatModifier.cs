using System;
using UnityEngine;

namespace StoryTime.Domains.Game.Characters.Modifiers
{
	/**
	 * @source https://youtu.be/SH25f3cXBVc
	 */
	public enum StatModType
	{
		Flat = 100, // Flexible to create values in between values if the user wants to.
		PercentAdd = 200,
		PercentMult = 300
	}

	[Serializable]
	public class StatModifier
	{
		public float Value => value;
		public StatModType Type => statType;

		public int Order => order;

		// Useful for sharing information what is providing the modifier.
		public readonly object Source;

		[SerializeField] private float value;
		[SerializeField] private StatModType statType;
		[SerializeField] private int order;

		public StatModifier() { }

		public StatModifier(float value, StatModType type, int index, object source = null)
		{
			this.value = value;
			statType = type;
			order = index;
			Source = source;
		}

		public StatModifier(float value, StatModType type): this(value, type, (int)type) { }
		public StatModifier(float value, StatModType type, object source): this(value, type, (int)type, source) { }
	}
}
