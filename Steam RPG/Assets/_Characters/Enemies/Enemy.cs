using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Wepons;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float chaseRadius = 8f;

        [SerializeField] float damagePerShot = 2f;
        [SerializeField] float firingPeriodInSeconds = 5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Projectile projetileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1.5f, 0);


        private float currentHealtPoints = 100f;
        AICharacterControl aiCharacterControl = null;
        Player player = null;
        bool isAttacking = false;

        private void Start()
        {
            currentHealtPoints = maxHealthPoints;
            player = GameObject.FindObjectOfType<Player>();
            aiCharacterControl = GetComponent<AICharacterControl>();
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if(player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this); //Stop enemie behavior
            }

            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelat = UnityEngine.Random.Range(firingPeriodInSeconds - firingPeriodVariation, firingPeriodInSeconds + firingPeriodVariation);
                InvokeRepeating("SpawnProjectile", 0f, randomisedDelat); // Switch to corutine 
            }

            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }


            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }
        }

        // TODO sperate firing logic 
        private void SpawnProjectile()
        {
            Projectile newProjectile = Instantiate(projetileToUse, projectileSocket.transform.position, Quaternion.identity);
            newProjectile.SetDamage(damagePerShot);
            newProjectile.SetShooter(this.gameObject);
            float projectilSpeed = newProjectile.projectileSpeed;

            Vector3 unitVectorToPlayer = Vector3.Normalize(player.transform.position + aimOffset - projectileSocket.transform.position);
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectilSpeed;
        }

        public float healthAsPercentage { get { return currentHealtPoints / maxHealthPoints; } }

        private void OnDrawGizmos()
        {

            // attack radius 
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, attackRadius);

            //move radius 
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseRadius);

        }

        public void TakeDamage(float damage)
        {
            currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);

            if (currentHealtPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}