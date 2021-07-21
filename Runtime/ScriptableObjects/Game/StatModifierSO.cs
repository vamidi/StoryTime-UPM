using System;
using UnityEngine;

namespace DatabaseSync.Game
{
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

		public readonly object Source;

		[SerializeField] private float value;
		[SerializeField] private StatModType statType;
		[SerializeField] private int order;

		public StatModifier() { }

		public StatModifier(float value, StatModType type, int index = 0)
		{
			this.value = value;
			statType = type;
			order = index;
		}
	}
}
