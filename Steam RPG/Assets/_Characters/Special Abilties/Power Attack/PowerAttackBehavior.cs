using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : MonoBehaviour, ISpecialAbilty
    {
        PowerAttack config;
        AudioSource audioSource = null;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void SetConfig(PowerAttack configToSet)
        {
            this.config = configToSet;
        }
        public void Use(AbiltyUseParams useParams)
        {
            DealExtraDamage(useParams);
            PlayParticleEffect();
            audioSource.clip = config.GetAduioClip();
            audioSource.Play(); // Move to somewhere to do not reapte our selfves 
        }
        private void PlayParticleEffect()
        {
            var prefab = Instantiate(config.GetParticalePrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(myParticleSystem, myParticleSystem.main.duration);
        }
        private void DealExtraDamage(AbiltyUseParams useParams)
        {
            float damageTodeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageTodeal);
        }
    }

}