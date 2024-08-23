using UnityEngine;

namespace StoryTime.Domains.Game.NPC.Enemies.Behaviours
{
    public class RigidbodyDelayedForce : MonoBehaviour
    {
        public Vector3 forceToAdd;

        private void Start()
        {
            Rigidbody[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < rigidbodies.Length; ++i)
            {
                rigidbodies[i].maxAngularVelocity = 45;
                rigidbodies[i].angularVelocity = transform.right * -45.0f;
                rigidbodies[i].velocity = forceToAdd;

            }
        }
    }
}
