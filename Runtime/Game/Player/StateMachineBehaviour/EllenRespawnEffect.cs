using UnityEngine;

namespace DatabaseSync.Components
{
    public class EllenRespawnEffect : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<PlayerController>().Respawn();
        }
    }
}
