using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {

        private GameObject player;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void LateUpdate()
        {
            this.transform.position = player.transform.position;
        }
    }
}
