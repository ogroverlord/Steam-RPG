using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{

    [SerializeField] float attackRadius = 4f;
    [SerializeField] float maxHealthPoints = 100f;

    private float currentHealtPoints = 100f;
    AICharacterControl aiCharacterControl = null;
    Player player = null; 

    private void Start()
    {
        player = (Player)GameObject.FindObjectOfType<Player>();
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(distanceToPlayer <= attackRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

    public float healthAsPercentage
{

    get
    {
        return currentHealtPoints / maxHealthPoints;
    }

}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRadius);
    }


}

