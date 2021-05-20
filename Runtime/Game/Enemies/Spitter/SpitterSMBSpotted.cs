using UnityEngine;

namespace DatabaseSync.Components
{
    public class SpitterSMBSpotted : SceneLinkedSMB<SpitterBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Spotted();
        }
    }
}
