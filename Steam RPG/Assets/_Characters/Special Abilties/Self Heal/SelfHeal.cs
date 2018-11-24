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
        
        public override AbiltyBehavior GetBehaviorComponent(GameObject gameObjectToattachTo)
        {
            return gameObjectToattachTo.AddComponent<SelfHealBehavior>();
        }
        public float GetHealValue()
        {
            return healAmount; 
        }
  
    }
}