using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Assertions;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] int enemyLayer = 9;
    [SerializeField] float damagePerHit = 10f; 
    [SerializeField] float minTimeBetweenHits = 0.5f; 
    [SerializeField] float maxAttackRange = 1.5f; 
    [SerializeField] float maxHealthPoints = 100f;

    [SerializeField] Wepon weponInUse; 


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
        RegisterForMouseClick();
        currentHealtPoints = maxHealthPoints;
        PutWeponInHand();   
    }

    private void PutWeponInHand()
    { 
        var weponPrefab = weponInUse.GetWeponPrefab();
        DominantHand dominanHand = RequestDominantHand();
        var wepon = Instantiate(weponPrefab, dominanHand.transform);
        wepon.transform.localPosition = weponInUse.gripTransform.localPosition; 
        wepon.transform.localRotation = weponInUse.gripTransform.localRotation; 
    }

    private DominantHand RequestDominantHand()
    {
        var dominanHand = GetComponentsInChildren<DominantHand>();

        int numberOfDominantHands = dominanHand.Length;

        Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found, pleas add one");
        Assert.IsFalse(numberOfDominantHands > 1, "More than one DominantHand found, only one can be set");
        return dominanHand[0];
    }

    private void RegisterForMouseClick()
    {
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
            
        }
    }
}
