using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Assertions;

// TODO Consier re-wiering 
using RPG.CameraUI; 
using RPG.Core;
using RPG.Wepons; 

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] int enemyLayer = 9;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Wepon weponInUse = null;
        [SerializeField] float damagePerHit = 10f;


        Animator animator; 
        float lastHitTime = 0f;
        GameObject currentTarget;
        CameraRaycaster cameraRaycaster;

        private float currentHealtPoints;
        public float healthAsPercentage
        {
            get { return currentHealtPoints / maxHealthPoints; }
        }

        private void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeponInHand();
            SetupRuntimeAnimator();
        }

        private void SetCurrentMaxHealth()
        {
            currentHealtPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEAFULT ATTACK"] = weponInUse.GetWeponAnimation(); //remove paramater const 
        }

        private void PutWeponInHand()
        {
            var weponPrefab = weponInUse.GetWeponPrefab();
            DominantHand dominanHand = RequestDominantHand();
            var wepon = Instantiate(weponPrefab, dominanHand.transform);
            wepon.transform.localPosition = weponInUse.gripTransform.localPosition;
            wepon.transform.localRotation = weponInUse.gripTransform.localRotation;
        }

        private DominantHand RequestDominantHand()
        {
            var dominanHand = GetComponentsInChildren<DominantHand>();

            int numberOfDominantHands = dominanHand.Length;

            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found, pleas add one");
            Assert.IsFalse(numberOfDominantHands > 1, "More than one DominantHand found, only one can be set");
            return dominanHand[0];
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClicked;

        }

        private void OnMouseClicked(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;
                if (IsEnemyInRange(enemy))
                {
                    AttackTarget(enemy); 
                }  
            }
        }

        private void AttackTarget(GameObject target)
        {
            if (Time.time - lastHitTime > weponInUse.GetMinTimeBetweenHits())
            {
                var enemyCompoent = target.GetComponent<Enemy>();
                enemyCompoent.TakeDamage(damagePerHit);
                animator.SetTrigger("Attacking"); 
                lastHitTime = Time.time;
            }
        }

        private bool IsEnemyInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weponInUse.GetMaxAttackRange();

        }

        public void TakeDamage(float damage)
        {

            currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
            if (currentHealtPoints <= 0f)
            {

            }
        }
    }
}