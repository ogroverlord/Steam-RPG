using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RPG.Core;


namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {


        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] takeDamageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float deathVanishSeconds = 0.5f;

        const string DEATH_TRIGER = "Death";

        float currentHealtPoints = 0;

        AudioSource audioSource;
        Animator animator;
        Character characterMovment;

        public float HealthAsPercentage
        {
            get { return currentHealtPoints / maxHealthPoints; }
        }


        private void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovment = GetComponent<Character>();

            currentHealtPoints = maxHealthPoints; 

        }

        private void Update()
        {
            UpdateHealtBar();
        }

        private void UpdateHealtBar()
        {
            if (healthBar)
            {
                healthBar.fillAmount = HealthAsPercentage;
            }
        }

        public void Heal(float healingAmount)
        {
            currentHealtPoints = Mathf.Clamp(currentHealtPoints + healingAmount, 0f, maxHealthPoints);
        }

        public void TakeDamage(float damage)
        {
            currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
            bool characterDies = (currentHealtPoints - damage <= 0);
            PlayTakeDamageSound();

            if (characterDies)
            {
                characterDies = false;
                StartCoroutine(KillCharacter());
            }
        }

        IEnumerator KillCharacter()
        {
            characterMovment.Kill();
            animator.SetTrigger(DEATH_TRIGER);

            var playerComponent = GetComponent<PlayerControl>(); 
            if(playerComponent && playerComponent.isActiveAndEnabled)
            {
                PlayDeathSound();
                yield return new WaitForSecondsRealtime(audioSource.clip.length + 1f);
                SceneManager.LoadScene(0); 
            }
            else 
            {
                Destroy(gameObject, deathVanishSeconds); 
            }


        }

        private void PlayDeathSound()
        {
            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length - 1)];
            audioSource.Play();
        }
        private void PlayTakeDamageSound()
        {
            var clip = takeDamageSounds[UnityEngine.Random.Range(0, takeDamageSounds.Length - 1)];
            audioSource.PlayOneShot(clip);
        }
    }
}