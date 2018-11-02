using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{

    public class AreaEffectBehavior : MonoBehaviour, ISpecialAbilty
    {

        AreaEffect config;
        AudioSource audioSource = null;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig(AreaEffect configToSet)
        {
            this.config = configToSet;
        }
        public void Use(AbiltyUseParams useParams)
        {
            DealRadialDamage(useParams);
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
        private void DealRadialDamage(AbiltyUseParams useParams)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                config.GetAoeRadius(),
                Vector3.up,
                config.GetAoeRadius()
            );

            float aoeDamage = config.GetExtraDamage() + useParams.baseDamage;

            foreach (RaycastHit hit in hits)
            {
                var damagable = hit.collider.gameObject.GetComponent<IDamageable>();
                var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (damagable != null && enemy)
                {
                    damagable.TakeDamage(aoeDamage);
                }
            }
        }

    }
}
