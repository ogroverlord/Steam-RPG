using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using UnityEngine.Assertions;

// TODO Consier re-wiering 
using RPG.CameraUI;
using RPG.Core;
using RPG.Wepons;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Wepon weponInUse = null;
        [SerializeField] float baseDamage = 10f;

        // temp serializer fileds 

        [SerializeField] SpecialAbilty[] abilities;
   

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
            abilities[0].AttachComponentTo(gameObject);
         
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
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

        }

        private void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) && IsEnemyInRange(enemy))
            {
                AttackTarget(enemy);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                UseSpecialAbilty(0,enemy);
            }

        }

        private void UseSpecialAbilty(int abiltyIndex, Enemy enemy)
        {
            var energyCost = abilities[abiltyIndex].GetEnergyCost();
            var energyComponent = gameObject.GetComponent<Energy>();
            if (energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbiltyUseParams(enemy, baseDamage);
                abilities[abiltyIndex].Use(abilityParams);
            }
        }

        private void AttackTarget(Enemy enemy)
        {
                if (Time.time - lastHitTime > weponInUse.GetMinTimeBetweenHits())
                {
                    enemy.TakeDamage(baseDamage);
                    animator.SetTrigger("Attacking");
                    lastHitTime = Time.time;
                }
        }



        private bool IsEnemyInRange(Enemy enemy)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
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
