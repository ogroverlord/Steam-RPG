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
        

        public override void AttachComponentTo(GameObject gameObjectToattachTo)
        {
            var behaviorComponent = gameObjectToattachTo.AddComponent<PowerAttackBehavior>();
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

       
        public float GetExtraDamage()
        {
            return extraDaamge; 
        }
        
    }
}