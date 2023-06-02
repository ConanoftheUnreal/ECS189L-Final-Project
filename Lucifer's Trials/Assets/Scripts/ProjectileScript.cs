using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float speed = 10.0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision");
        // All projectiles that hit a wall should be destroyed.
        if (col.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        // Enemy projectiles that hit the player get destroyed
        if (this.tag == "EnemyProjectile" && col.tag == "PlayerHurtbox")
        {
            Destroy(this.gameObject);
            col.transform.parent.GetComponent<PlayerController>().DecreaseHealth(1);
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
