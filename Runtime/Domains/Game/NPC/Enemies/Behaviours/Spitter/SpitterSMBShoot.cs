using UnityEngine;

namespace StoryTime.Domains.Game.NPC.Enemies.Behaviours.Spitter
{
    public class SpitterSMBShoot : SceneLinkedSMB<SpitterBehaviour>
    {
        static int s_IdleStateHash = Animator.StringToHash("Idle");
        protected Vector3 m_AttackPosition;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (m_MonoBehaviour.Target == null)
            {
                //if we reached the shooting state without a target, mean the target move outside of our detection range
                //so just go back to idle.
                animator.Play(s_IdleStateHash);
                return;
            }


            m_MonoBehaviour.Controller.SetFollowNavmeshAgent(false);

            m_MonoBehaviour.RememberTargetPosition();
            Vector3 toTarget = m_MonoBehaviour.Target.transform.position - m_MonoBehaviour.transform.position;
            toTarget.y = 0;

            m_MonoBehaviour.transform.forward = toTarget.normalized;
            m_MonoBehaviour.Controller.SetForward(m_MonoBehaviour.transform.forward);

            if (m_MonoBehaviour.attackAudio != null)
                m_MonoBehaviour.attackAudio.PlayRandomClip();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.FindTarget();
        }
    }
}
