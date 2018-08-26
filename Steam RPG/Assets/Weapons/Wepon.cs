using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = ("RPG/Wepon"))]
public class Wepon : ScriptableObject
{

    [SerializeField] GameObject weponPrefab;
    [SerializeField] GameObject atackAnimation;

    public Transform gripTransform; 


    public GameObject GetWeponPrefab()
    {
        return weponPrefab; 
    }
}

