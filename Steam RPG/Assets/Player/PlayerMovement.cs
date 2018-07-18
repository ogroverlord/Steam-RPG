using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;


[RequireComponent(typeof(ThirdPersonCharacter), typeof(CameraRaycaster))]
public class PlayerMovement : MonoBehaviour
{
    private bool isInDirectMode = false;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float atackMoveStopRadius = 5f;
    [SerializeField] float atackRadius = 1.5f;
    


    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }
    //TO DO corect WASD so it cannot affect movment 

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))  //allow player to change mapping, general menus
        {
            isInDirectMode = !isInDirectMode;  // togle mode 

            currentDestination = transform.position;
            if (isInDirectMode) print("Direct Mode ON!");
            else print("Direct Mode OFF!");
        }

        if (isInDirectMode)
        {
            ProcesDirectMovment();
        }
        else
        {
            ProcesMouseMovment();
        }
    }

    private void ProcesMouseMovment()
    {
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.layerHit.point;

            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;

                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, atackMoveStopRadius);

                    break;

                default:
                    print("Uknown layer!");
                    return;
            }
        }
        WalkToDestion();

    }

    private void WalkToDestion()
    {
        Vector3 playerToClickPoint = currentDestination - gameObject.transform.position;

        if (playerToClickPoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

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
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(clickPoint, 0.2f);

        //atack spheara
        Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, atackMoveStopRadius);

        //meele attack radius 
        //Gizmos.color = new Color(0f, 100f, 250f, 0.5f);
        //Gizmos.DrawSphere(transform.position, atackRadius);

    }

}


