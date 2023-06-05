using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class ProjectileScript : MonoBehaviour
{
    private float speed = 10.0f;
    [SerializeField] int damage = 3;

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
            // get `Player` gameobject from collider of `PlayerHurtbox` and hurt player
            col.transform.parent.GetComponent<PlayerController>().DecreaseHealth(damage);
            col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.RANGED);
            Destroy(this.gameObject);
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
