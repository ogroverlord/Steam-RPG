using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbiltyBehavior
    {


        public override void Use(AbiltyUseParams useParams)
        {
            PlayAbiltySound();
            DealExtraDamage(useParams);
            PlayParticleEffect();

        }
       
        private void DealExtraDamage(AbiltyUseParams useParams)
        {
            float damageTodeal = useParams.baseDamage + (specialAbilty as PowerAttack).GetExtraDamage();
            useParams.target.TakeDamage(damageTodeal);
        }
    }

}