using UnityEngine;

namespace RPG.Characters
{
    public class FaceCamera : MonoBehaviour
    {

        Camera cameraToLookAt;

        void Start()
        {
            cameraToLookAt = Camera.main;
        }

        void LateUpdate()
        {
            var Up = Vector3.up;
            transform.LookAt(cameraToLookAt.transform, Up);
        }
    }
}