using System;
using UnityEngine;
using UnityEngine.Localization;

using StoryTime.Database.ScriptableObjects;
namespace StoryTime.Domains.Game.Characters.ScriptableObjects
{
	/// <summary>
	///
	/// </summary>
	public enum SkillType
	{
		Attack,
		Magic,
		Guard,
		Items
	}

	/// <summary>
	///
	/// </summary>
	public enum AttributeType
	{
		HP,
		MP,
		TP,
		SP
	}

	/// <summary>
	///
	/// </summary>
	public enum DamageType
	{
		Damage,
		Recover,
		Drain,
	}

	[CreateAssetMenu(fileName = "Skill", menuName = "StoryTime/Game/Characters/Skill", order = 4)]
	// ReSharper disable once InconsistentNaming
	public partial class SkillSO : LocalizationBehaviour
	{
		public LocalizedString SkillName
		{
			get => skillName;
			internal set => skillName = value;
		}

		public LocalizedString Description
		{
			get => description;
			internal set => description = value;
		}

		public Attribute MagicAttribute
		{
			get => magicParameter;
			internal set => magicParameter = value;
		}

		public SkillType SkillType
		{
			get => skillType;
			internal set => skillType = value;
		}

		public int MagicCost
		{
			get => magicCost;
			internal set => magicCost = value;
		}

		public double Speed
		{
			get => speed;
			internal set => speed = value;
		}

		public uint Repeat
		{
			get => repeat;
			internal set => repeat = value;
		}

		public Attribute DamageAttribute
		{
			get => damageParameter;
			internal set => damageParameter = value;
		}

		public DamageType DamageType
		{
			get => type;
			internal set => type = value;
		}

		public string Formula
		{
			get => formula;
			internal set => formula = value;
		}

		public double Variance
		{
			get => variance;
			internal set => variance = value;
		}

		public float SuccessRate
		{
			get => successRate;
			internal set => successRate = value;
		}

		// skill. = successRate;
		// skill. = criticalHits;

		public bool CriticalChance
		{
			get => criticalChance;
			internal set => criticalChance = value;
		}

		public int Level
		{
			get => level;
			internal set => level = value;
		}

		[Header("General")]
		[SerializeField, HideInInspector, Tooltip("Used for validation to where this skill belongs to.")] protected uint classId;
		[SerializeField] private LocalizedString skillName;
		[SerializeField] private LocalizedString description;

		[SerializeField, Tooltip("Can this skill critical hit")] protected bool criticalChance = false;
		[SerializeField, Tooltip("Required level for this skill")] protected int level = 1;

		[SerializeField, Tooltip("The type (category) of the skill")] protected SkillType skillType = SkillType.Attack;
		[SerializeField, Tooltip("Attribute we are going to use to subtract the magic cost")] protected Attribute magicParameter = new ();
		[SerializeField] protected int magicCost = Int32.MaxValue;
		[SerializeField, Tooltip("How many enemies/players can this skill hit")] protected int scope = Int32.MaxValue;

		[Header("Invocation")]
		[SerializeField, Tooltip("How many enemies/players can this skill hit")] protected double speed = Int32.MaxValue;
		[SerializeField, Tooltip("Calculation rate")] protected float successRate = Int32.MaxValue;
		[SerializeField, Tooltip("How many times the skill should be repeated")] protected uint repeat = Int32.MaxValue;

		[Header("Damage Settings")]
		[SerializeField, Tooltip("Attribute we are going to attack/heal on.")] protected Attribute damageParameter = new ();
		[SerializeField, Tooltip("Type of damage this skill can do")] protected DamageType type = DamageType.Damage;
		[SerializeField, Tooltip("Formula we are going to use to calculate our dmg/rec/drain")] protected string formula = "";
		[SerializeField, Tooltip("How much off we can be from the actual dmg/rec/drain (use this for randomness)")] protected double variance = 0f;

		SkillSO(): base("skills", "name") {}

		protected override void OnTableIDChanged()
		{
			base.OnTableIDChanged();
			Initialize();
		}

		public void OnEnable()
		{
#if UNITY_EDITOR
			Initialize();
#endif
		}

		private void Initialize()
		{

		}
	}
}
