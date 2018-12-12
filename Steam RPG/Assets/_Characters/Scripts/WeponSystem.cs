using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;



namespace RPG.Characters
{
    public class WeponSystem : MonoBehaviour
    {

        [SerializeField] Wepon currentWeponConfig;
        [SerializeField] float baseDamage = 10f;

        const string ATTACK_TRIGER = "Attacking";
        const string DEAFULT_ATTACK = "DEAFULT ATTACK";

        GameObject weponObject;
        GameObject target;
        Animator animator;
        Character character;
        float lastHitTime = 0f;


        private void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            PutWeponInHand(currentWeponConfig);
            SetAttackAnimation();
        }
        private float CalculateDamage()
        {
            return (baseDamage + currentWeponConfig.GetAdditionalDamage());
        }
        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            var animatorOverrideController = character.GetAnimatorOverrideController();

            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEAFULT_ATTACK] = currentWeponConfig.GetWeponAnimation(); //remove paramater const 
        }
        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
              //  this.enemy.TakeDamage(CalculateDamage());
                animator.SetTrigger(ATTACK_TRIGER);
                lastHitTime = Time.time;
            }
        }
        private DominantHand RequestDominantHand()
        {
            var dominanHand = GetComponentsInChildren<DominantHand>();

            int numberOfDominantHands = dominanHand.Length;

            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found, pleas add one");
            Assert.IsFalse(numberOfDominantHands > 1, "More than one DominantHand found, only one can be set");
            return dominanHand[0];
        }


        public Wepon GetCurrentWepon()
        {
            return currentWeponConfig;
        }
        public void PutWeponInHand(Wepon weponToUse)
        {
            currentWeponConfig = weponToUse;
            var weponPrefab = currentWeponConfig.GetWeponPrefab();
            DominantHand dominanHand = RequestDominantHand();
            Destroy(weponObject);
            weponObject = Instantiate(weponPrefab, dominanHand.transform);
            weponObject.transform.localPosition = currentWeponConfig.gripTransform.localPosition;
            weponObject.transform.localRotation = currentWeponConfig.gripTransform.localRotation;

            //audioSource.clip = wepoinToUse.GetWeponPickupSound();
            //audioSource.Play();
        }
        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            print("attacking: " + targetToAttack);
            //use corutine to attack reapatidly
        }

    }
}