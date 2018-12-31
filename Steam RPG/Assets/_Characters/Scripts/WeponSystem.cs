using System;
using System.Collections;
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

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            PutWeponInHand(currentWeponConfig);
            SetAttackAnimation();
        }
        void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;

            if (target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                targetIsDead = (target.GetComponent<HealthSystem>().HealthAsPercentage <= Mathf.Epsilon);

                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget  > currentWeponConfig.GetMaxAttackRange();

                bool characterIsDead = (GetComponent<HealthSystem>().HealthAsPercentage <= Mathf.Epsilon);

                print(targetIsDead + " " + targetIsOutOfRange + " " + characterIsDead);

                if (targetIsDead || targetIsOutOfRange || characterIsDead)
                {
                    StopAllCoroutines();
                }
            }


        }

        internal void StopAttacking()
        {
            animator.StopPlayback();
            StopAllCoroutines();
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
            //if (Time.time - lastHitTime > currentWeponConfig.GetMinTimeBetweenHits())
            //{
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGER);
                lastHitTime = Time.time;
            //}
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
            StartCoroutine(AttackTargetRepeatedly());
        }

        IEnumerator AttackTargetRepeatedly()
        {
            bool attackerStillAlive = GetComponent<HealthSystem>().HealthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().HealthAsPercentage >= Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive)
            {
                float weponHitPeriod = currentWeponConfig.GetMinTimeBetweenHits();
                float timeToWait = weponHitPeriod * character.GetAnimationSpeedMultiplyer();

                bool isItTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if (isItTimeToHitAgain)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        private void AttackTargetOnce()
        {
            if (!character.GetAnimatorOverrideController())
            {
                Debug.Break();
                Debug.LogAssertion("Plese provide " + gameObject + " with animator override controler");
            }
            else
            {
                transform.LookAt(target.transform);
                animator.SetTrigger(ATTACK_TRIGER);
                float damageDelay = currentWeponConfig.GetDamageDeley(); // Get from wepon 
                SetAttackAnimation();
                StartCoroutine(DamageAfterDelay(damageDelay));
            }
        }

        private IEnumerator DamageAfterDelay(float damageDelay)
        {
            yield return new WaitForSeconds(damageDelay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
        }
    }
}
