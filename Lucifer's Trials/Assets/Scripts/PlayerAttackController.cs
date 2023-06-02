using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class PlayerAttackController : MonoBehaviour
{
    private float projectileCooldown;
    private bool projectileReloading;
    private float projectileDuration;
    [SerializeField] GameObject playerProjectile;

    void Start()
    {
        this.projectileCooldown = 0.5f;
        this.projectileReloading = false;
        this.projectileDuration = 0.0f;
    }

    void Update()
    {
        PlayerType playerClass = this.GetComponent<PlayerAnimationController>().GetPlayerType();

        switch (playerClass)
        {
            case PlayerType.WARRIOR:
                break;

            case PlayerType.SORCERESS:
                if (Input.GetButtonDown("Fire1") && !this.projectileReloading)
                {
                    this.projectileReloading = !this.projectileReloading;

                    if (this.gameObject.GetComponent<PlayerMovement>().GetFacedDirection() == "Down")
                    {
                        var projectile = (GameObject)Instantiate(playerProjectile, transform.position, Quaternion.Euler(0, 0, 180));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                    }
                    if (this.gameObject.GetComponent<PlayerMovement>().GetFacedDirection() == "Left")
                    {
                        var projectile = (GameObject)Instantiate(playerProjectile, transform.position, Quaternion.Euler(0, 0, 90));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                    }
                    if (this.gameObject.GetComponent<PlayerMovement>().GetFacedDirection() == "Up")
                    {
                        var projectile = (GameObject)Instantiate(playerProjectile, transform.position, Quaternion.Euler(0, 0, 0));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                    }
                    if (this.gameObject.GetComponent<PlayerMovement>().GetFacedDirection() == "Right")
                    {
                        var projectile = (GameObject)Instantiate(playerProjectile, transform.position, Quaternion.Euler(0, 0, 270));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                    }
                }
                break;

            default:
                Debug.Log("Error: No player class assigned to the player.");
                break;
        }

        if (this.projectileReloading)
        {
            this.projectileDuration += Time.deltaTime;
            if (this.projectileDuration > this.projectileCooldown)
            {
                this.projectileReloading = !this.projectileReloading;
                this.projectileDuration = 0.0f;
            }
        }
    }
}