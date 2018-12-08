using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {

        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorControler;
        [SerializeField] AnimatorOverrideController overwriteControler;
        [SerializeField] Avatar characterAvatar;

        [Header("Audio")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter;
        [SerializeField] float coliderRadius;
        [SerializeField] float coliderHeight;

        [Header("Movment")]
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float animationSpeedMultiplier = 1f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 0.1f;

        [Header("NavMesh Agent")]
        [SerializeField] float navMeshAgentSteeringSpeed = 1f;
        [SerializeField] float navMeshAgentStopingDistance = 1.3f;



        private bool isInDirectMode = false;
        float turnAmount;
        float forwardAmount;
        ThirdPersonCharacter character;
        Vector3 clickPoint;
        Animator animator;
        Rigidbody myRigidbody;
        NavMeshAgent navMeshAgent;
        bool isAlive = true;


        private void Awake()
        {
            AddRequierdComponents();
        }

        private void AddRequierdComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = coliderRadius;
            capsuleCollider.height = coliderHeight;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorControler;
            animator.avatar = characterAvatar;

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshAgentStopingDistance;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = false;
            navMeshAgent.stoppingDistance = stoppingDistance;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;
        }


        void Update()
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }



        void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                velocity.y = myRigidbody.velocity.y;
                myRigidbody.velocity = velocity;
            }
        }

        Vector3 ShortDestination(Vector3 destination, float shortening)
        {
            Vector3 reductionVector = (destination - transform.position).normalized * shortening;
            return destination - reductionVector;
        }

        public void SetDestination(Vector3 worldPosition)
        {
            navMeshAgent.destination = worldPosition;
        }

        private void Move(Vector3 movment)
        {
            SetForwardAndTurn(movment);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        private void SetForwardAndTurn(Vector3 movment)
        {
            if (movment.magnitude > moveThreshold)
            {
                movment.Normalize();
            }

            var localMove = transform.InverseTransformDirection(movment);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }
        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }
        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        public void Kill()
        {
            isAlive = false;
        }
    }


}