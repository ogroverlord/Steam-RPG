using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abilty/SelfHeal"))]
    public class SelfHeal : SpecialAbilty 
    {
        [Header("Self Heal Specific")]
        [SerializeField] float healAmount = 50f;
        

        public override void AttachComponentTo(GameObject gameObjectToattachTo)
        {
            var behaviorComponent = gameObjectToattachTo.AddComponent<SelfHealBehavior>();
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

       
        public float GetHealValue()
        {
            return healAmount; 
        }

    
        
    }
}