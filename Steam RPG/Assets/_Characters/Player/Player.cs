using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// TODO Consier re-wiering 
using RPG.CameraUI;
using RPG.Core;


namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Wepon currentWeponConfig = null;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] AudioClip[] takeDamageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplayer = 2f;
        [SerializeField] ParticleSystem criticalHitParticle;

        // temp serializer fileds 
        [SerializeField] SpecialAbilty[] abilities;

        const string DEATH_TRIGER = "Death";
        const string ATTACK_TRIGER = "Attacking";
        const string DEAFULT_ATTACK = "DEAFULT ATTACK";

        Enemy enemy = null;
        AudioSource audioSource = null;
        Animator animator = null;
        float lastHitTime = 0f;
        GameObject currentTarget = null;
        CameraRaycaster cameraRaycaster = null;
        GameObject weponObject = null; 
       

        private float currentHealtPoints;
        public float healthAsPercentage
        {
            get { return currentHealtPoints / maxHealthPoints; }
        }

        private void Start()
        {
    
            audioSource = gameObject.GetComponent<AudioSource>();

            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeponInHand(currentWeponConfig);
            SetAttackAnimation();
            AttachInitialAbilities();

        }
        private void Update()
        {
            if (healthAsPercentage > Mathf.Epsilon)
            {
                ScanForAbiltyKeyDown();
            }
        }
        private void AttachInitialAbilities()
        {
            for (int abiltyIndex = 0; abiltyIndex < abilities.Length; abiltyIndex++)
            {
                abilities[abiltyIndex].AttachAbiltyTo(gameObject);
            }
        }
        private void ScanForAbiltyKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                UseSpecialAbilty(1);
            }
            else if(Input.GetKeyDown(KeyCode.Keypad2))
            {
                UseSpecialAbilty(2);
            }
        }
        private void SetCurrentMaxHealth()
        {
            currentHealtPoints = maxHealthPoints;
        }
        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEAFULT_ATTACK] = currentWeponConfig.GetWeponAnimation(); //remove paramater const 
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
        private void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;

            if (Input.GetMouseButton(0) && IsEnemyInRange(enemyToSet))
            {
                AttackTarget();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                UseSpecialAbilty(0);
            }

        }
        private void UseSpecialAbilty(int abiltyIndex)
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
        }
        private bool IsEnemyInRange(Enemy enemy)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeponConfig.GetMaxAttackRange();

        }
        public void TakeDamage(float amountOfDamage)
        {
            currentHealtPoints = Mathf.Clamp(currentHealtPoints - amountOfDamage, 0f, maxHealthPoints);
            bool playerDies = currentHealtPoints <= 0f;
            PlayTakeDamageSound();

            if (playerDies)
            {
                playerDies = false;
                animator.SetTrigger(DEATH_TRIGER);
                StartCoroutine(KillPlayer());
            }

        }
        public void Heal(float healingAmount)
        {
            currentHealtPoints = Mathf.Clamp(currentHealtPoints + healingAmount, 0f, maxHealthPoints);
        }
        private void PlayDeathSound()
        {
            var radndomIdex = UnityEngine.Random.Range(0, deathSounds.Length - 1);
            audioSource.clip = deathSounds[radndomIdex];
            audioSource.Play();
        }
        private void PlayTakeDamageSound()
        {
            var radndomIdex = UnityEngine.Random.Range(0, takeDamageSounds.Length - 1);
            audioSource.clip = takeDamageSounds[radndomIdex];
            audioSource.Play();
        }
        IEnumerator KillPlayer()
        {
            PlayDeathSound();
            print("Player Dies");

            yield return new WaitForSecondsRealtime(audioSource.clip.length + 1f); //TODO use aduio clip length 

            SceneManager.LoadScene(0);
        }
    }
}
