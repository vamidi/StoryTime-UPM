using UnityEngine;

namespace StoryTime.Components
{
    public class SpitterSMBCooldown : SceneLinkedSMB<SpitterBehaviour>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.FindTarget();
            m_MonoBehaviour.CheckNeedFleeing();
        }
    }
}
