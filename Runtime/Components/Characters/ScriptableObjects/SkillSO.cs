using System;

using UnityEngine;
using UnityEngine.Localization;

namespace DatabaseSync.Components
{
	public enum DamageType
	{
		Damage,
		Recover,
		Drain,
	}

	[Serializable]
	public class Stats
	{
		public string alias = "";
	}

	[CreateAssetMenu(fileName = "Skill", menuName = "DatabaseSync/Game/Characters/Skill", order = 0)]
	// ReSharper disable once InconsistentNaming
	public partial class SkillSO : LocalizationBehaviour
	{
		[Header("General")]
		[SerializeField, HideInInspector, Tooltip("Used for validation to where this skill belongs to.")] protected uint classId;
		[SerializeField, Tooltip("Can this skill critical hit")] protected bool criticalChance = false;
		[SerializeField] private LocalizedString skillName;
		[SerializeField] private LocalizedString description;
		[SerializeField, Tooltip("Required level for this skill")] protected int level = 1;
		[SerializeField] protected int magicCost = Int32.MaxValue;
		[SerializeField, Tooltip("How many enemies/players can this skill hit")] protected int scope = Int32.MaxValue;

		[Header("Invocation")]
		[SerializeField, Tooltip("How many enemies/players can this skill hit")] protected float speed = Int32.MaxValue;
		[SerializeField, Tooltip("Calculation rate")] protected float successRate = Int32.MaxValue;
		[SerializeField, Tooltip("How many times the skill should be repeated")] protected uint repeat = Int32.MaxValue;

		[Header("Damage Settings")]
		[SerializeField, Tooltip("Stats we are going to attack/heal on.")] protected Stats parameter = null;
		[SerializeField, Tooltip("Type of damage this skill can do")] protected DamageType type = DamageType.Damage;
		[SerializeField, Tooltip("Formula we are going to use to calculate our dmg/rec/drain")] protected string formula = "";
		[SerializeField, Tooltip("How much off we can be from the actual dmg/rec/drain (use this for randomness)")] protected float variance = 0f;

		SkillSO(): base("skills", "name") {}
	}
}
