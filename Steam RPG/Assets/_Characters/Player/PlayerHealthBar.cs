﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Characters
{
    [RequireComponent(typeof(Image))]
    public class PlayerHealthBar : MonoBehaviour
    {

        Image healthOrbImage;
        Player player;

        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<Player>();
            healthOrbImage = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            healthOrbImage.fillAmount = player.healthAsPercentage;
        }
    }
}