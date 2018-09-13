using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Wepons
{
    [CreateAssetMenu(menuName = ("RPG/Wepon"))]
    public class Wepon : ScriptableObject
    {

        [SerializeField] GameObject weponPrefab;
        [SerializeField] AnimationClip atackAnimation;
        [SerializeField] float maxAttackRange = 1.5f;
        [SerializeField] float minTimeBetweenHits = 0.5f;

        public Transform gripTransform;

        public float GetMinTimeBetweenHits()
        {
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
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

        //So that assets pack cannot caouse crashes 
        private void RemoveAnimationEvents()
        {
            atackAnimation.events = new AnimationEvent[0];
        }
    }

}