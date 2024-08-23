﻿using UnityEngine;

namespace StoryTime.Domains.Game.Characters.Players.StateMachine
{
    public class EllenSetTargetableSMB : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController controller = animator.GetComponent<PlayerController>();

            if (controller != null)
            {
                controller.RespawnFinished();
            }
        }
    }
}
