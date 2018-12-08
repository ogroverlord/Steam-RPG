using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehavior : AbiltyBehavior
    {
        PlayerMovment player = null;


        private void Start()
        {
            player = GetComponent<PlayerMovment>();
        }

        public override void Use(GameObject target)
        {
            PlayAbiltySound();
            HealPlayer(target);
            PlayParticleEffect();
        }

        private void HealPlayer(GameObject target)
        {
            var playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.Heal((specialAbilty as SelfHeal).GetHealValue());
        }
    }

}