using UnityEngine;

namespace StoryTime.Domains.Game.NPC.Enemies.Behaviours.Spitter
{
    public class SpitterSMBSpotted : SceneLinkedSMB<SpitterBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Spotted();
        }
    }
}
