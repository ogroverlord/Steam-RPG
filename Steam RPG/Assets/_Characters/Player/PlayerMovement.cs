using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI; // TODO Consier re-wiering 

namespace RPG.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovement : MonoBehaviour
    {
        private bool isInDirectMode = false;

        ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        Vector3 clickPoint;
        AICharacterControl aiCharacterControl = null;
        GameObject walkTarget = null;

        private void Start()
        {
            aiCharacterControl = GetComponent<AICharacterControl>();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");

            cameraRaycaster.onMouseOverWalkable += ProcessWalkToMove;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(0))
            {
                aiCharacterControl.SetTarget(enemy.transform);
            }
        }

        private void ProcessWalkToMove(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                aiCharacterControl.SetTarget(walkTarget.transform);
            }
        }

        //make it get called again 
        private void ProcesDirectMovment()
        {
            float horitzontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 cameraForward;
            Vector3 movment;

            // calculate camera relative direction to move:
            cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            movment = vertical * cameraForward + horitzontal * Camera.main.transform.right;

            thirdPersonCharacter.Move(movment, false, false);
        }

        Vector3 ShortDestination(Vector3 destination, float shortening)
        {
            Vector3 reductionVector = (destination - transform.position).normalized * shortening;
            return destination - reductionVector;
        }



    }


}