using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{

    public class AreaEffectBehavior : AbiltyBehavior
    {



        public override void Use(GameObject target)
        {
            PlayAbiltySound();
            DealRadialDamage();
            PlayParticleEffect();
 
        }

        private void DealRadialDamage()
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (specialAbilty as AreaEffect).GetAoeRadius(),
                Vector3.up,
                (specialAbilty as AreaEffect).GetAoeRadius()
            );

            float aoeDamage = (specialAbilty as AreaEffect).GetExtraDamage();

            foreach (RaycastHit hit in hits)
            {
                var damagable = hit.collider.gameObject.GetComponent<HealthSystem>();
                var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (damagable != null && enemy)
                {
                    damagable.TakeDamage(aoeDamage);
                }
            }
        }

    }
}
