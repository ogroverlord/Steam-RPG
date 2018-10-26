using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{

    public class AreaEffectBehavior : MonoBehaviour, ISpecialAbilty
    {

        AreaEffect config;

        private void Start()
        {
            print(this + " compoent behavior was added to Player");
        }

        public void SetConfig(AreaEffect configToSet)
        {
            this.config = configToSet;
        }

        public void Use(AbiltyUseParams useParams)
        {
            print("Power attack used, extra damage: " + gameObject.name);
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                config.GetAoeRadius(),
                Vector3.up,
                config.GetAoeRadius()
            );

            float aoeDamage = config.GetExtraDamage() + useParams.baseDamage;

            foreach (RaycastHit hit in hits)
            {
                var damagable = hit.collider.gameObject.GetComponent<IDamageable>();
                var enemie = hit.collider.gameObject.GetComponent<Enemy>();
                if(damagable != null && enemie)
                {
                    damagable.TakeDamage(aoeDamage);
                }
            }

        }

    
    }
}
