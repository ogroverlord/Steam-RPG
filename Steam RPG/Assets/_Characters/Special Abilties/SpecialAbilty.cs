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

        protected ISpecialAbilty behavior;

        abstract public void AttachComponentTo(GameObject gameObjectToattachTo);

        public void Use(AbiltyUseParams useParams)
        {
            behavior.Use(useParams);
        }


        public float GetEnergyCost()
        {
            return energyCost;
        }
    }

    public interface ISpecialAbilty
    {

        void Use(AbiltyUseParams useParams);

    }
}