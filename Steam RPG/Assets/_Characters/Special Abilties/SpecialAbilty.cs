using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
  
    public abstract class SpecialAbilty : ScriptableObject
    {
        [Header("Special Abilty General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particalePrefab = null;
        [SerializeField] AudioClip[] audioClips;
        [SerializeField] AnimationClip abiltyAnimation;


        protected AbiltyBehavior behavior;

       
        public void Use(GameObject target)
        {
            behavior.Use(target);
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

        public AnimationClip GetAbiltyAnimation()
        {
            return abiltyAnimation;
        }
    }

}