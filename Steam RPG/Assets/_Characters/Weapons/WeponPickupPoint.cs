﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WeponPickupPoint : MonoBehaviour
    {
        [SerializeField] Wepon weponConfig;

        private void Start()
        {
            InstantiateWepon();
        }

        private void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        private void InstantiateWepon()
        {
            var wepon = weponConfig.GetWeponPrefab();
            wepon.transform.position = Vector3.zero;
            Instantiate(wepon, gameObject.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerControl>())
            {
                FindObjectOfType<PlayerControl>().GetComponent<WeponSystem>().PutWeponInHand(weponConfig);
            }
        }
    }
}