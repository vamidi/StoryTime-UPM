using UnityEngine;
using UnityEngine.Localization;

using StoryTime.Domains.Game.Characters;
using StoryTime.Domains.Game.Characters.ScriptableObjects;

namespace StoryTime.Editor.Wizards
{
	public class SkillWizard : BaseWizard<SkillWizard, SkillSO>
	{
		[Header("General settings")]
		[SerializeField, Tooltip("Skill name")] protected LocalizedString name = new ();
		[SerializeField, Tooltip("Skill description")] protected LocalizedString description = new ();
		[SerializeField, Tooltip("Skill type")] protected SkillType skillType = SkillType.Attack;
		// TODO create
		[SerializeField, Tooltip("Skill parameter")] protected Attribute magicParameter = new ();
		[SerializeField, Tooltip("Skill cost")] protected int magicCost = 0;
		[SerializeField, Tooltip("Required level for this skill")] protected int level = 1;

		[Header("Invocation")]
		[SerializeField, Tooltip("Will determine how fast the player can do this skill")] protected int speed = 0;
		[SerializeField, Tooltip("Will determine the success rate whether the skill will hit")] protected float successRate = 0.0f;
		[SerializeField, Tooltip("How many times this skill can repeat itself")] protected uint repeat = 0;

		[Header("Damage Settings")]
		[SerializeField, Tooltip("Damage parameter")] protected Attribute damageParameter = new ();
		[SerializeField, Tooltip("How many times this skill can repeat itself")] protected DamageType damageType = DamageType.Damage;
		[SerializeField, Tooltip("How many times this skill can repeat itself")] protected string formula = "";
		[SerializeField, Tooltip("How many times this skill can repeat itself")] protected int variance = 0;
		[SerializeField, Tooltip("How many times this skill can repeat itself")] protected bool criticalChance = false;

		public override void OnWizardUpdate()
		{
			helpString = "Create a new Skill scriptable object";

			base.OnWizardUpdate();
		}

		protected override bool Validate()
		{
			if (name.IsEmpty)
			{
				errorString = "Please fill in a name";
				return false;
			}

			if (damageType == DamageType.Damage && damageParameter.attributeName.IsEmpty)
			{
				errorString = "Damage Type is selected, but Damage Attribute name or alias are not complete...";
				return false;
			}

			if (skillType != SkillType.Attack && magicParameter.attributeName.IsEmpty)
			{
				errorString = "Skill Type is selected, but Damage Attribute name or alias are not complete...";
				return false;
			}

			errorString = "";
			return true;
		}

		protected override SkillSO Create(string location)
		{
			var skill = CreateInstance<SkillSO>();
			skill.SkillName = name;
			skill.Description = description;
			skill.MagicAttribute = magicParameter;
			skill.SkillType = skillType;
			skill.MagicCost = magicCost;
			skill.Level = level;

			skill.Speed = speed;
			skill.SuccessRate = successRate;
			skill.Repeat = repeat;

			skill.DamageAttribute = damageParameter;
			skill.DamageType = damageType;
			skill.Formula = formula;
			skill.Variance = variance;
			skill.CriticalChance = criticalChance;

			return skill;
		}
	}
}
