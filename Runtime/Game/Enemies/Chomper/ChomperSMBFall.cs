using UnityEngine;

namespace DatabaseSync.Components
{
	// ReSharper disable once InconsistentNaming
	public class ChomperSMBFall : SceneLinkedSMB<ChomperBehavior>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Adding force will disable the navmesh agent & move through the rigidbody.
            // Since we only want the chomper to fall, we add zero force, the call just allow to make it move through rigidbody
            if(m_MonoBehaviour != null && m_MonoBehaviour.Controller != null)
                m_MonoBehaviour.Controller.AddForce(Vector3.zero);
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Controller.ClearForce();
        }
    }
}
