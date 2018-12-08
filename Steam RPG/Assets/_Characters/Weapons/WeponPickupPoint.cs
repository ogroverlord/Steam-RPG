using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    //[ExecuteInEditMode]
    public class WeponPickupPoint : MonoBehaviour
    {
        [SerializeField] Wepon weponConfig;

        private void Start()
        {
            InstantiateWepon();
        }

        void Update()
        {
            //if (!Application.isPlaying)
            //{
            //    DestroyChildren();
            //    InstantiateWepon();
            //}
        }

        private void DestroyChildren()
        {
   
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }

        }

        void InstantiateWepon()
        {
            var wepon = weponConfig.GetWeponPrefab();
            wepon.transform.position = Vector3.zero;
            Instantiate(wepon, gameObject.transform);
        }
        private void OnTriggerEnter(Collider other)
        {
            FindObjectOfType<PlayerMovment>().PutWeponInHand(weponConfig);
        }
    }
}