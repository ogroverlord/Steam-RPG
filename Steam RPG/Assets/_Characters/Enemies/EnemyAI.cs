﻿using System;
using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent (typeof(HealthSystem))]
    [RequireComponent(typeof(WeponSystem))]
    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 8f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2f;

        Character character;
        PlayerControl player = null;
        float currentWeponRange = 4f;
        float distanceToPlayer;
        int nextWaypointIndex = 0; 
        float waitTime = 0.5f; 

        enum State
        {
            attacking,
            idle,
            patrolling,
            chasing
        }

        State state = State.idle;

        private void Start()
        {
            player = GameObject.FindObjectOfType<PlayerControl>();
            character = gameObject.GetComponent<Character>();
        }

        private void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeponSystem weponSystem = GetComponent<WeponSystem>();
            currentWeponRange = weponSystem.GetCurrentWepon().GetMaxAttackRange();

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                weponSystem.StopAttacking();
                StartCoroutine(Patrol());
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeponRange && state != State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
                weponSystem.AttackTarget(player.gameObject);

            }
        }

      

        IEnumerator Patrol()
        {
            state = State.patrolling;

            while (true)
            {
                Vector3 nextWaypointPosition = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPosition);
                CycleWaypointWhenClose(nextWaypointPosition);
                yield return new WaitForSeconds(waitTime);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPosition)
        {
            if (Vector3.Distance(transform.position, nextWaypointPosition) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmos()
        {

            // attack radius 
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, currentWeponRange);

            //move radius 
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseRadius);

        }
    }

}