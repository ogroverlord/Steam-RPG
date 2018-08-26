using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;


[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    private bool isInDirectMode = false;

    //solve const and serialize fields
    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

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
        cameraRaycaster.notifyMouseClickObservers += ProcessMouseCLick;
    }

    private void ProcessMouseCLick(RaycastHit raycastHit, int layerHit)
    {
        switch (layerHit)
        {

            case enemyLayerNumber:
                //navigate to enemy
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                //walk to part of terrain 
                break;
            
            default:
                Debug.LogError("Do not know how to handel this case!"); 
                break;
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

    void OnDrawGizmos()
    {
        //movment gizmos
        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(transform.position, currentDestination);
        //Gizmos.DrawSphere(currentDestination, 0.1f);
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(clickPoint, 0.2f);

        //atack spheara
        //Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
        //Gizmos.DrawWireSphere(transform.position, atackMoveStopRadius);

        //meele attack radius 
        //Gizmos.color = new Color(0f, 100f, 250f, 0.5f);
        //Gizmos.DrawSphere(transform.position, atackRadius);

    }

}


