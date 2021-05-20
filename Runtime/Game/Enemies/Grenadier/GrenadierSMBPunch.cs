
using UnityEngine;

namespace DatabaseSync.Components
{
    public class GrenadierSMBPunch : SceneLinkedSMB<GrenadierBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (m_MonoBehaviour.punchAudioPlayer)
                m_MonoBehaviour.punchAudioPlayer.PlayRandomClip();
        }
    }
}
