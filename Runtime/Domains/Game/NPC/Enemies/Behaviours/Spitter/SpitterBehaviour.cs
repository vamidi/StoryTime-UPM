
using UnityEditor;
using UnityEngine;

namespace StoryTime.Domains.Game.NPC.Enemies.Behaviours.Spitter
{
    using StoryTime.Domains.Game.Audio;
    using StoryTime.Domains.Game.Cameras;
    using StoryTime.Domains.Game.Weapons;
    using StoryTime.Domains.Game.DamageSystem;
    
    
    [DefaultExecutionOrder(100)]
    public class SpitterBehaviour : EnemyBehaviour, IMessageReceiver
    {
        public static readonly int hashVerticalDot = Animator.StringToHash("VerticalHitDot");
        public static readonly int hashHorizontalDot = Animator.StringToHash("HorizontalHitDot");
        public static readonly int hashThrown = Animator.StringToHash("Thrown");
        public static readonly int hashHit = Animator.StringToHash("Hit");
        public static readonly int hashAttack = Animator.StringToHash("Attack");
        public static readonly int hashHaveEnemy = Animator.StringToHash("HaveTarget");
        public static readonly int hashFleeing = Animator.StringToHash("Fleeing");

        public static readonly int hashIdleState = Animator.StringToHash("Idle");

        public TargetScanner playerScanner;
        public float fleeingDistance = 3.0f;
        public RangeWeapon rangeWeapon;

        [Header("Audio")]
        public RandomAudioPlayer attackAudio;
        public RandomAudioPlayer frontStepAudio;
        public RandomAudioPlayer backStepAudio;
        public RandomAudioPlayer hitAudio;
        public RandomAudioPlayer gruntAudio;
        public RandomAudioPlayer deathAudio;
        public RandomAudioPlayer spottedAudio;

        protected bool m_Fleeing = false;

        protected Vector3 m_RememberedTargetPosition;

        protected override void OnEnable()
        {
	        base.OnEnable();

	        m_EnemyController.animator.Play(hashIdleState, 0, Random.value);

            SceneLinkedSMB<SpitterBehaviour>.Initialise(m_EnemyController.animator, this);
        }

        public override void OnReceiveMessage(MessageType type, object sender, object msg)
        {
            switch (type)
            {
                case MessageType.Dead:
	                Death((Damageable.DamageMessage)msg);
                    break;
                case MessageType.Damaged:
	                ApplyDamage((Damageable.DamageMessage)msg);
                    break;
                default:
                    break;
            }
        }

        public void Death(Damageable.DamageMessage msg)
        {
            Vector3 pushForce = transform.position - msg.damageSource;

            pushForce.y = 0;

            transform.forward = -pushForce.normalized;
            m_EnemyController.AddForce(pushForce.normalized * 7.0f - Physics.gravity * 0.6f);

            m_EnemyController.animator.SetTrigger(hashHit);
            m_EnemyController.animator.SetTrigger(hashThrown);

            //We unparent the deathAudio source, as it would destroy it with the gameobject when it get replaced by the ragdol otherwise
            deathAudio.transform.SetParent(null, true);
            deathAudio.PlayRandomClip();

            GameObject.Destroy(deathAudio, deathAudio.clip == null ? 0.0f : deathAudio.clip.length + 0.5f);
        }

        public void ApplyDamage(Damageable.DamageMessage msg)
        {
            if (msg.damager.name == "Staff")
                CameraShake.Shake(0.06f, 0.1f);

            float verticalDot = Vector3.Dot(Vector3.up, msg.direction);
            float horizontalDot = Vector3.Dot(transform.right, msg.direction);

            Vector3 pushForce = transform.position - msg.damageSource;

            pushForce.y = 0;

            transform.forward = -pushForce.normalized;
            m_EnemyController.AddForce(pushForce.normalized * 5.5f, false);

            m_EnemyController.animator.SetFloat(hashVerticalDot, verticalDot);
            m_EnemyController.animator.SetFloat(hashHorizontalDot, horizontalDot);

            m_EnemyController.animator.SetTrigger(hashHit);

            hitAudio.PlayRandomClip();
        }

        public void Shoot()
        {
            rangeWeapon.Attack(m_RememberedTargetPosition);
        }

        public void TriggerAttack()
        {
            m_EnemyController.animator.SetTrigger(hashAttack);
        }

        public void RememberTargetPosition()
        {
            if (m_Target == null)
                return;

            m_RememberedTargetPosition = m_Target.transform.position;
        }

        void PlayStep(int frontFoot)
        {
            if (frontStepAudio != null && frontFoot == 1)
                frontStepAudio.PlayRandomClip();
            else if (backStepAudio != null && frontFoot == 0)
                backStepAudio.PlayRandomClip ();
        }

        public void Grunt ()
        {
            if (gruntAudio != null)
                gruntAudio.PlayRandomClip ();
        }

        public void Spotted()
        {
            if (spottedAudio != null)
                spottedAudio.PlayRandomClip();
        }

        public void CheckNeedFleeing()
        {
            if (m_Target == null)
            {
                m_Fleeing = false;
                m_EnemyController.animator.SetBool(hashFleeing, m_Fleeing);
                return;
            }

            Vector3 fromTarget = transform.position - m_Target.transform.position;

            if (m_Fleeing || fromTarget.sqrMagnitude <= fleeingDistance * fleeingDistance)
            {
                //player is too close from us, pick a point diametrically oppossite at twice that distance and try to move there.
                Vector3 fleePoint = transform.position + fromTarget.normalized * 2 * fleeingDistance;

                Debug.DrawLine(fleePoint, fleePoint + Vector3.up * 10.0f);

                if (!m_Fleeing)
                {
                    //if we're not already fleeing, we may be in the cooldown, so the navmesh agent is disabled, enable it
                    m_EnemyController.SetFollowNavmeshAgent(true);
                }

                m_Fleeing = m_EnemyController.SetTarget(fleePoint);

                if (m_Fleeing)
	                m_EnemyController.animator.SetBool(hashFleeing, m_Fleeing);
            }

            if (m_Fleeing && fromTarget.sqrMagnitude > fleeingDistance * fleeingDistance * 4)
            {
                //we're twice the fleeing distance from the player and fleeing, we can stop now
                m_Fleeing = false;
                m_EnemyController.animator.SetBool(hashFleeing, m_Fleeing);
            }
        }

        public void FindTarget()
        {
            //we ignore height difference if the target was already seen
            m_Target = playerScanner.Detect(transform, m_Target == null);
            m_EnemyController.animator.SetBool(hashHaveEnemy, m_Target != null);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            playerScanner.EditorGizmo(transform);
        }
#endif
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(SpitterBehaviour))]
    public class SpitterBehaviourEditor : Editor
    {
        SpitterBehaviour m_Target;

        void OnEnable()
        {
            m_Target = target as SpitterBehaviour;
        }

        public override void OnInspectorGUI()
        {
            if (m_Target.playerScanner.detectionRadius < m_Target.fleeingDistance)
            {
                EditorGUILayout.HelpBox("The scanner detection radius is smaller than the fleeing range.\n" +
                    "The spitter will never shoot at the player as it will flee past the range at which it can see the player",
	                UnityEditor.MessageType.Warning, true);
            }

            base.OnInspectorGUI();
        }
    }

#endif
}
