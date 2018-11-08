using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehavior : MonoBehaviour, ISpecialAbilty
    {
        SelfHeal config = null;
        Player player = null;
        AudioSource audioSource = null;

        private void Start()
        {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }
        public void SetConfig(SelfHeal configToSet)
        {
            this.config = configToSet;
        }
        public void Use(AbiltyUseParams useParams)
        {
            HealPlayer(useParams);
            PlayParticleEffect();
        }
        private void PlayParticleEffect()
        {
            var particalePrefab = config.GetParticalePrefab();
            var prefab = Instantiate(config.GetParticalePrefab(), transform.position, particalePrefab.transform.rotation);
            prefab.transform.parent = player.transform;
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(myParticleSystem, myParticleSystem.main.duration);
        }
        private void HealPlayer(AbiltyUseParams useParams)
        {
            player.Heal(config.GetHealValue());
            audioSource.clip = config.GetAduioClip();
            audioSource.Play(); 
        }
    }

}