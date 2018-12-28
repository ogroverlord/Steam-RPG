using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbiltyBehavior
    {


        public override void Use(GameObject target)
        {
            PlayAbiltySound();
            DealExtraDamage(target);
            PlayParticleEffect();
            PlayAbiltyAnimation();

        }
       
        private void DealExtraDamage(GameObject target)
        {
            float damageTodeal = (specialAbilty as PowerAttack).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageTodeal);   
        }
    }

}