using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehavior : AbiltyBehavior
    {
        Player player = null;


        private void Start()
        {
            player = GetComponent<Player>();
        }

        public override void Use(AbiltyUseParams useParams)
        {
            PlayAbiltySound();
            HealPlayer(useParams);
            PlayParticleEffect();
        }

        private void HealPlayer(AbiltyUseParams useParams)
        {
            player.Heal((specialAbilty as SelfHeal).GetHealValue());
        }
    }

}