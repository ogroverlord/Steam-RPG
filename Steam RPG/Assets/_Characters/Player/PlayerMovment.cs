
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class PlayerMovment : MonoBehaviour
    {
 
        Character character;
        EnemyAI enemy;
        AudioSource audioSource;
        SpecialAbilities abilities;
        CameraRaycaster cameraRaycaster;
        WeponSystem weponSystem;

    
        private void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            audioSource = GetComponent<AudioSource>();
            weponSystem = GetComponent<WeponSystem>();
            RegisterForMouseEvents();    
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
        
        private void OnMouseOverEnemy(EnemyAI enemy)
        {
            this.enemy = enemy;

            if (Input.GetMouseButton(0) && IsEnemyInRange(enemy))
            {
                weponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.UseSpecialAbilty(0);
            }
        }

        private bool IsEnemyInRange(EnemyAI enemy)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= weponSystem.GetCurrentWepon().GetMaxAttackRange();

        }

    }
}
