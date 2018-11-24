using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abilty/Area Effect"))]
    public class AreaEffect : SpecialAbilty
    {

        [Header("AOE specifics")]
        [SerializeField] float extraDaamge = 10f;
        [SerializeField] float aoeRadius = 4f;


        public override AbiltyBehavior GetBehaviorComponent(GameObject gameObjectToattachTo)
        {
            return gameObjectToattachTo.AddComponent<AreaEffectBehavior>();
        }
        public float GetExtraDamage()
        {
            return extraDaamge;
        }
        public float GetAoeRadius()
        {
            return aoeRadius; 
        }

    }
}
