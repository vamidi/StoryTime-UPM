using UnityEngine;

namespace DatabaseSync.Components
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
