using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed = 10f;
    float damage;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider collider)
    {
        IDamageable damagableComponent = collider.gameObject.GetComponent<IDamageable>();
            
        if(damagableComponent != null)
        {
            damagableComponent.TakeDamage(damage);
        }
    }

    


}
