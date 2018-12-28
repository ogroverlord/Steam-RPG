using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehavior : AbiltyBehavior
    {
        PlayerControl player = null;


        private void Start()
        {
            player = GetComponent<PlayerControl>();
        }

        public override void Use(GameObject target)
        {
            PlayAbiltySound();
            HealPlayer(target);
            PlayParticleEffect();
            PlayAbiltyAnimation();
        }

        private void HealPlayer(GameObject target)
        {
            var playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.Heal((specialAbilty as SelfHeal).GetHealValue());
        }
    }

}