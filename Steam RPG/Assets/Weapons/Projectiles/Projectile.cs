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

    
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damagableComponent = collision.gameObject.GetComponent<IDamageable>();
            
        if(damagableComponent != null)
        {
            damagableComponent.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }

    


}
