
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// TODO Consier re-wiering 
using RPG.CameraUI;
using System;

namespace RPG.Characters
{
    public class PlayerMovment : MonoBehaviour
    {
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Wepon currentWeponConfig = null;
        [SerializeField] float baseDamage = 10f;
        [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplayer = 2f;
        [SerializeField] ParticleSystem criticalHitParticle;

        // temp serializer fileds 

        const string ATTACK_TRIGER = "Attacking";
        const string DEAFULT_ATTACK = "DEAFULT ATTACK";

        Character character;
        Enemy enemy;
        Animator animator;
        AudioSource audioSource;
        SpecialAbilities abilities;
        float lastHitTime = 0f;
        CameraRaycaster cameraRaycaster;
        GameObject weponObject;



        private void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            audioSource = GetComponent<AudioSource>();
            RegisterForMouseEvents();
            PutWeponInHand(currentWeponConfig);
            SetAttackAnimation();
        }

        private void Update()
        {
           ScanForAbiltyKeyDown();
        }

        
        private void ScanForAbiltyKeyDown()
        {
            for (int keyIndex = 0; keyIndex <= abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.UseSpecialAbilty(keyIndex);
                }
            }
        }

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEAFULT_ATTACK] = currentWeponConfig.GetWeponAnimation(); //remove paramater const 
        }

        private DominantHand RequestDominantHand()
        {
            var dominanHand = GetComponentsInChildren<DominantHand>();

            int numberOfDominantHands = dominanHand.Length;

            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found, pleas add one");
            Assert.IsFalse(numberOfDominantHands > 1, "More than one DominantHand found, only one can be set");
            return dominanHand[0];
        }

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverWalkable += OnMouseOverWalkable;

        }

        private void OnMouseOverWalkable(Vector3 destination)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }
        
        private void OnMouseOverEnemy(Enemy enemy)
        {
            this.enemy = enemy;

            if (Input.GetMouseButton(0) && IsEnemyInRange(enemy))
            {
                AttackTarget();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.UseSpecialAbilty(0);
            }
        }
       
        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                this.enemy.TakeDamage(CalculateDamage());
                animator.SetTrigger(ATTACK_TRIGER);
                lastHitTime = Time.time;
            }
        }
        private float CalculateDamage()
        {
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;

            if (isCriticalHit)
            {
                criticalHitParticle.Play();
                return criticalHitMultiplayer * (baseDamage + currentWeponConfig.GetAdditionalDamage());
            }
            else
            {
                return (baseDamage + currentWeponConfig.GetAdditionalDamage());
            }
        } //wepojns 
        bool IsEnemyInRange(Enemy enemy)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeponConfig.GetMaxAttackRange();

        }
        public void PutWeponInHand(Wepon wepoinToUse)
        {
            currentWeponConfig = wepoinToUse;
            var weponPrefab = wepoinToUse.GetWeponPrefab();
            DominantHand dominanHand = RequestDominantHand();
            Destroy(weponObject);
            weponObject = Instantiate(weponPrefab, dominanHand.transform);
            weponObject.transform.localPosition = currentWeponConfig.gripTransform.localPosition;
            weponObject.transform.localRotation = currentWeponConfig.gripTransform.localRotation;

            audioSource.clip = wepoinToUse.GetWeponPickupSound();
            audioSource.Play();
        }
    
    }
}
