using UnityEngine;
using UnityEngine.Animations.Rigging;

#if DOTWEEN
using DG.Tweening;
#endif

namespace StoryTime.Domains.Game.Abilities
{
	public class ParticleAbility : AbilityBase
	{
		[Header("Connections")] [SerializeField]
		ParticleSystem particle = default;

		[SerializeField] private TwoBoneIKConstraint shootRig = default;

		public override void Ability()
		{
			particle.Play();

			if (!shootRig)
				return;

#if ODIN_INSPECTOR
			DOVirtual.Float(0, 1, .1f, (x) => shootRig.weight = x)
				.OnComplete(() => DOVirtual.Float(1, 0, .3f, (x) => shootRig.weight = x));
		#endif
		}
	}
}
