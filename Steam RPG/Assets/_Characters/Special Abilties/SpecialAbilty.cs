using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public struct AbiltyUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbiltyUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }
    public abstract class SpecialAbilty : ScriptableObject
    {
        [Header("Special Abilty General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particalePrefab = null;
        [SerializeField] AudioClip[] audioClips;


        protected AbiltyBehavior behavior;

       
        public void Use(AbiltyUseParams useParams)
        {
            behavior.Use(useParams);
        }
        public float GetEnergyCost()
        {
            return energyCost;
        }
        public GameObject GetParticalePrefab()
        {
            return particalePrefab;
        }
        public AudioClip GetAduioClips()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        public abstract AbiltyBehavior GetBehaviorComponent(GameObject gameObjectToattachTo);

        public void AttachAbiltyTo(GameObject gameObjectToattachTo)
        {
            AbiltyBehavior behaviorComponent = GetBehaviorComponent(gameObjectToattachTo);
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }
    }

}