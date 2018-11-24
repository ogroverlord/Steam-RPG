using UnityEngine;

namespace RPG.Characters
{
	
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;
		[SerializeField] float moveThreshold = 0.1f;

		Rigidbody myRigidbody;
		float turnAmount;
		float forwardAmount;
        Animator animator;


        void Start()
		{
            animator = GetComponent<Animator>();
            myRigidbody = GetComponent<Rigidbody>();
			myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
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
            forwardAmount = movment.z;
        }

        void UpdateAnimator()
		{
			animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
		}

        void ApplyExtraTurnRotation()
		{
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
		}
			
	}
}
