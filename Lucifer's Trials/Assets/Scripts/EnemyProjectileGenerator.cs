using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemyProjectile;
    private float arrowRotationFix = -45.0f;
    private float cooldown = 2.0f;
    private float duration = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (duration > cooldown)
        {
            var projectile = (GameObject)Instantiate(enemyProjectile, transform.position, Quaternion.Euler(0, 0, 270 + arrowRotationFix));
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
            duration = 0.0f;
        }
        else
        {
            duration += Time.deltaTime;
        }
    }
}
