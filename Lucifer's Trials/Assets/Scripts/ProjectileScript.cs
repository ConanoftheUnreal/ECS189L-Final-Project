using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float speed = 10.0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision");
        if (col.tag == "Wall")
        {
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
