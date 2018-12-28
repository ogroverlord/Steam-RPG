
using UnityEngine;
using RPG.CameraUI;
using System;
using System.Collections;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
 
        Character character;
        AudioSource audioSource;
        SpecialAbilities abilities;
        WeponSystem weponSystem;

    
        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            audioSource = GetComponent<AudioSource>();
            weponSystem = GetComponent<WeponSystem>();
            RegisterForMouseEvents();    
        }
        void Update()
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
        private void RegisterForMouseEvents()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
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
        
        private void OnMouseOverEnemy(EnemyAI enemy)
        {
        
            if (Input.GetMouseButton(0) && IsEnemyInRange(enemy))
            {
                weponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButton(0) && !IsEnemyInRange(enemy))
            {
                StartCoroutine(MoveAndAttack(enemy));
            }    
            else if (Input.GetMouseButtonDown(1) && IsEnemyInRange(enemy))
            {
                abilities.UseSpecialAbilty(0, enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1) && !IsEnemyInRange(enemy))
            {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        private IEnumerator MoveToEnemy(EnemyAI enemy)
        {
            while (!IsEnemyInRange(enemy))
            {
                character.SetDestination(enemy.gameObject.transform.position);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();

        }

        private IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToEnemy(enemy));
            weponSystem.AttackTarget(enemy.gameObject); 
        }

        private IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToEnemy(enemy));
            abilities.UseSpecialAbilty(0, enemy.gameObject);
        }

        private bool IsEnemyInRange(EnemyAI enemy)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= weponSystem.GetCurrentWepon().GetMaxAttackRange();

        }

    }
}
