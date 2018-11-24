using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abilty/Power Attack"))]
    public class PowerAttack : SpecialAbilty 
    {
        [Header("Power Attack Specific")]
        [SerializeField] float extraDaamge = 10f;

        public override AbiltyBehavior GetBehaviorComponent(GameObject gameObjectToattachTo)
        {
            return gameObjectToattachTo.AddComponent<PowerAttackBehavior>();
        }
        public float GetExtraDamage()
        {
            return extraDaamge; 
        }
        
    }
}