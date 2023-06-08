using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] int damage = 3;
    [SerializeField] ProjectileTypes projectileType;
    [SerializeField] GameObject afterEffectPrefab;
    private float speed = 10.0f;

    public void OnTriggerEnter2D(Collider2D col)
    {
        // All projectiles that hit a wall should be destroyed.
        if (col.tag == "Wall")
        {
            Destroy(this.gameObject);
            AfterEffect();
        }

        // Enemy projectiles that hit the player get destroyed
        if (this.tag == "EnemyProjectile" && col.tag == "PlayerHurtbox")
        {
            // get `Player` gameobject from collider of `PlayerHurtbox` and hurt player
            col.transform.parent.GetComponent<PlayerController>().DecreaseHealth(damage);
            col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.RANGED);
            Destroy(this.gameObject);
            AfterEffect();
        }
    }

    private void AfterEffect()
    {
        GameObject projectile;
        switch (projectileType)
        {
            case ProjectileTypes.FIRE:
                // fire objects that hit a wall will cause ignition at collision
                projectile = Instantiate(this.afterEffectPrefab, this.gameObject.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                break;
            case ProjectileTypes.PHYSICAL:
                // break effect or similar
                break;
            default:
                Debug.Log("Error: Invalid projectile type.");
                break;
        }
    }

    public float GetProjectileSpeed()
    {
        return this.speed;
    }

}
