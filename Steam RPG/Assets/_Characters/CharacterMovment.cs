using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMovment : MonoBehaviour
    {
        private bool isInDirectMode = false;
        		
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float animationSpeedMultiplier = 1f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 0.1f;

        float turnAmount;
        float forwardAmount;
        Animator animator;
        ThirdPersonCharacter character;
        Vector3 clickPoint;
        Rigidbody myRigidbody;
        NavMeshAgent agent; 

        private void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

            myRigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            animator.applyRootMotion = true; 
            agent.updatePosition = true; 
            agent.updateRotation = false;
            agent.stoppingDistance = stoppingDistance;
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;


            cameraRaycaster.onMouseOverWalkable += ProcessWalkToMove;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }
        void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                Move(agent.desiredVelocity); 
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(0))
            {
                agent.SetDestination(enemy.transform.position);
            }
        }
        private void ProcessWalkToMove(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
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

        public void Move(Vector3 movment)
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

    }


}