using UnityEngine;

namespace StoryTime.Components
{
    public class SpitterSMBFleeing : SceneLinkedSMB<SpitterBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Controller.SetFollowNavmeshAgent(true);
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.FindTarget();
            m_MonoBehaviour.CheckNeedFleeing();
        }
    }
}
