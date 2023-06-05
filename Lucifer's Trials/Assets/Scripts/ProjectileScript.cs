using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class ProjectileScript : MonoBehaviour
{
    private float speed = 10.0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Collision");
        // All projectiles that hit a wall should be destroyed.
        if (col.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        // Enemy projectiles that hit the player get destroyed
        if (this.tag == "EnemyProjectile" && col.tag == "PlayerHurtbox")
        {
            // damage set here as default; can be an attribute of the EnemyProjectile game object in general
            int damage = 1;
            col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.RANGED);
            Destroy(this.gameObject);
            // get `Player` gameobject from collider of `PlayerHurtbox` and hurt player
            col.transform.parent.GetComponent<PlayerController>().DecreaseHealth(damage);
        }
    }

    public float GetProjectileSpeed()
    {
        return this.speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
