using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StoryTime.Domains.Attributes;
using UnityEngine;
using UnityEngine.Localization;

namespace StoryTime.Domains.Game.Characters
{
	using Utils.Attributes;

	[Serializable]
	public partial class CharacterStats
	{
		/// <summary>
		/// ID of the row inside the table.
		/// </summary>
		public String ID
		{
			get => id;
			set => id = value;
		}

		public String ParamID => paramId;
		public LocalizedString StatName => statName;
		public Attribute Attribute => attribute;
		public string Formula => paramFormula;

		public float Flat => flat;

		public float Rate => rate;

		public ReadOnlyCollection<StatModifier> StatModifiers => statModifiers.AsReadOnly();

		public float Value
		{
			get
			{
				if (_isDirty || Math.Abs(baseValue - _lastBaseValue) > 0)
				{
					_lastBaseValue = baseValue;
					_value = Calculate();
					_isDirty = false;
				}

				return _value;
			}
		}

		public bool IsLocalized => isLocalized;
		public LocalizedSprite LocalizePreviewImage => localizePreviewImage;
		public Sprite PreviewImage => previewImage;

		/**
		 * Database variables
		 */
		[SerializeField, Uuid] private String id;
		[SerializeField] private String paramId;
		[SerializeField] protected LocalizedString statName;
		// TODO better naming
		[SerializeField] protected Attribute attribute = new ();
		[SerializeField] protected string paramFormula = "";
		[SerializeField] protected float flat = 0f;
		[SerializeField] protected float rate = 0f;

		[SerializeField] protected bool isLocalized;
		[SerializeField, ConditionalField("overrideDescriptionTable")] protected LocalizedSprite localizePreviewImage;
		[SerializeField] protected Sprite previewImage;
		public float baseValue;
		[SerializeField] protected List<StatModifier> statModifiers = new List<StatModifier>();

		private bool _isDirty = true;
		private float _lastBaseValue = float.MinValue;
		private float _value;

		public void Add(StatModifier mod)
		{
			_isDirty = true;
			statModifiers.Add(mod);
			statModifiers.Sort((a, b) =>
			{
				if (a.Order < b.Order)
					return -1;

				if (a.Order > b.Order)
					return 1;

				return 0;
			});
		}

		public bool Remove(StatModifier mod)
		{
			 _isDirty = statModifiers.Remove(mod);
			 return _isDirty;
		}

		public void Clear()
		{
			statModifiers.Clear();
		}

		public bool RemoveAllFromSource(object source)
		{
			bool didRemove = false;
			for(int i = statModifiers.Count -1; i >= 0; i--)
			{
				if (statModifiers[i].Source == source)
				{
					_isDirty = true;
					didRemove = true;
					statModifiers.RemoveAt(i);
				}
			}

			return didRemove;
		}

		private float Calculate()
		{
			float finalValue = baseValue;
			float sumPercentAdd = 0;

			for (int i = 0; i < StatModifiers.Count; i++)
			{
				var mod = StatModifiers[i];
				switch (mod.Type)
				{
					case StatModType.Flat:
						finalValue += mod.Value;
						break;
					case StatModType.PercentAdd:
						sumPercentAdd += mod.Value;
						if (i + 1 >= StatModifiers.Count || StatModifiers[i + 1].Type != StatModType.PercentAdd)
						{
							finalValue *= 1 + sumPercentAdd;
							sumPercentAdd = 0;
						}
						break;
					case StatModType.PercentMult:
						finalValue *= 1 + mod.Value;
						break;
				}
			}

			// 12.0001f != 12f
			return (float) Math.Round(finalValue, 4);
		}
	}
}
