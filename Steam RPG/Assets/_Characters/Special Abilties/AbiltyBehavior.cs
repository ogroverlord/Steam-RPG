using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public abstract class AbiltyBehavior : MonoBehaviour
    {
        protected SpecialAbilty specialAbilty;
        const float PARTICLE_CLEAN_UP_DELAY = 20f;

        public abstract void Use(AbiltyUseParams useParams);

        public void SetConfig(SpecialAbilty specialAbiltyToSet)
        {
            specialAbilty = specialAbiltyToSet;
        }
        protected void PlayParticleEffect()
        {
            var particalePrefab = specialAbilty.GetParticalePrefab();
            var particleObject = Instantiate(particalePrefab,
                                     transform.position,
                                     particalePrefab.transform.rotation);
            particleObject.transform.parent = transform; 
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
       }
        protected void PlayAbiltySound()
        {
            var abiltySound = specialAbilty.GetAduioClips();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abiltySound); 
        }
        IEnumerator DestroyParticleWhenFinished(GameObject particalePrefab)
        {
            while (particalePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }

            Destroy(particalePrefab);

            yield return new WaitForEndOfFrame();
        }
    }
}
