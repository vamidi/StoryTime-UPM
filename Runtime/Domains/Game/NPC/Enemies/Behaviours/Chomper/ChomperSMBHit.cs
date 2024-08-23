using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryTime.Domains.Game.NPC.Enemies.Behaviours.Chomper
{
    public class ChomperSMBHit : SceneLinkedSMB<ChomperBehavior>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger(ChomperBehavior.hashAttack);
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Controller.ClearForce();
        }
    }
}
