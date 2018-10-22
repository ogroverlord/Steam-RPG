using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : MonoBehaviour, ISpecialAbilty
    {
        PowerAttack config;

        private void Start()
        {
            print(this + " compoent behavior was added to Player");
        }

        public void SetConfig(PowerAttack configToSet)
        {
            this.config = configToSet;
        }

        public void Use(AbiltyUseParams useParams)
        {
            print("Power attack used, extra damage: " + gameObject.name);
            float damageTodeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageTodeal); 
        }


    }

}