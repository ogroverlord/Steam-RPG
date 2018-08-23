using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int enemyLayer = 9;
    [SerializeField] float damagePerHit = 10f; 
    [SerializeField] float minTimeBetweenHits = 0.5f; 
    [SerializeField] float maxAttackRange = 1.5f; 
    [SerializeField] float maxHealthPoints = 100f;

    float lastHitTime = 0f;
    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;

    private float currentHealtPoints;
    public float healthAsPercentage
    {
        get { return currentHealtPoints / maxHealthPoints; }
    }

    private void Start()
    {
        currentHealtPoints = maxHealthPoints; 
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClicked;
    }

    private void OnMouseClicked(RaycastHit raycastHit, int layerHit)
    {
       if(layerHit == enemyLayer )
        {
            var enemy = raycastHit.collider.gameObject;
            var enemyCompoent = enemy.GetComponent<Enemy>();

             
            if((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return;
            }

            if (Time.time - lastHitTime > minTimeBetweenHits)
            {
                currentTarget = enemy;
                enemyCompoent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }
    }

    public void TakeDamage(float damage)
    {

        currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
        if (currentHealtPoints <= 0f)
        {
            print("End Game");
        }
    }
}
