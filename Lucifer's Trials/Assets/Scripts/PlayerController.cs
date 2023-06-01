using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int health;
    private int SP;
    private float projectileCooldown;
    private bool projectileReloading;
    private float projectileDuration;
    [SerializeField] GameObject playerProjectile;

    // Start is called before the first frame update
    void Start()
    {
        this.health = 3;
        this.SP = 0;
        this.projectileCooldown = 0.5f;
        this.projectileReloading = false;
        this.projectileDuration = 0.0f;
    }

    public void IncreaseHealth(int amount)
    {
        this.health += amount;
        Debug.Log(this.health);
    }
    public void IncreaseSP(int amount)
    {
        this.SP += amount;
        Debug.Log(this.SP);
    }
    public int GetHealth()
    {
        return this.health;
    }
    public int GetSP()
    {
        return this.SP;
    }
    // Update is called once per frame
    void Update()
    {
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
