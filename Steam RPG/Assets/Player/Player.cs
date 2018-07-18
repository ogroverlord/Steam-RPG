using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    [SerializeField] float maxHealthPoints = 100f;
    private float currentHealtPoints = 100f;


    public float healthAsPercentage
    {

        get
        {
            return currentHealtPoints / maxHealthPoints;
        }

    }

}
