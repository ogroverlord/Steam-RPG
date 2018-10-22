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


        public override void AttachComponentTo(GameObject gameObjectToattachTo)
        {
            var behaviorComponent = gameObjectToattachTo.AddComponent<AreaEffectBehavior>();
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
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
