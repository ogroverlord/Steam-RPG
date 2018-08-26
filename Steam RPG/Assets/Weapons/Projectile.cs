using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    const float DELAY = 0.02f;

    public float projectileSpeed = 10f;
    private float damage;

    [SerializeField] GameObject shooter;


    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public float GetDefoultLanuchSpeed()
    {
        return projectileSpeed;
    }
    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var layerCollidedWith = collision.gameObject.layer;
        if (layerCollidedWith != shooter.layer)
        {
            DamageIfDemagable(collision);
        }
    }


    private void DamageIfDemagable(Collision collision)
    {
        IDamageable damagableComponent = collision.gameObject.GetComponent<IDamageable>();

        if (damagableComponent != null)
        {
            damagableComponent.TakeDamage(damage);
            Destroy(this.gameObject, DELAY);
        }
    }



}
