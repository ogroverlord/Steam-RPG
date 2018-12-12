using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Wepon"))]
    public class Wepon : ScriptableObject
    {

        [SerializeField] GameObject weponPrefab;
        [SerializeField] AnimationClip atackAnimation;
        [SerializeField] AudioClip pickupSFX;
        [SerializeField] float maxAttackRange = 1.5f;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float additionalDamage = 10f;

        public Transform gripTransform;

        public float GetMinTimeBetweenHits()
        {
            return minTimeBetweenHits;
        }
        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }
        public float GetAdditionalDamage()
        {
            return additionalDamage; 
        }
        public GameObject GetWeponPrefab()
        {
            return weponPrefab;
        }
        public AnimationClip GetWeponAnimation()
        {
            RemoveAnimationEvents();
            return atackAnimation;
        }
        public AudioClip GetWeponPickupSound()
        {
            return pickupSFX;
        }

        private void RemoveAnimationEvents()
        {
            atackAnimation.events = new AnimationEvent[0];
        }
    }

}