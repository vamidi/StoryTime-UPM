using UnityEngine;

namespace StoryTime.Components
{
    public class ChomperSMBAttack : SceneLinkedSMB<ChomperBehavior>
    {
        protected Vector3 m_AttackPosition;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);

            m_MonoBehaviour.Controller.SetFollowNavmeshAgent(false);

            m_AttackPosition = m_MonoBehaviour.target.transform.position;
            Vector3 toTarget = m_AttackPosition - m_MonoBehaviour.transform.position;
            toTarget.y = 0;

            m_MonoBehaviour.transform.forward = toTarget.normalized;
            m_MonoBehaviour.Controller.SetForward(m_MonoBehaviour.transform.forward);

            if (m_MonoBehaviour.attackAudio != null)
                m_MonoBehaviour.attackAudio.PlayRandomClip();
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex);

            if (m_MonoBehaviour.attackAudio != null)
                m_MonoBehaviour.attackAudio.audioSource.Stop();

            m_MonoBehaviour.Controller.SetFollowNavmeshAgent(true);
        }
    }
}
