using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int health;
    private float projectileCooldown;
    private bool projectileReloading;
    private float projectileDuration;
    private enum PlayerType { WARRIOR, SORCERESS }
    [SerializeField] private PlayerType playerType = PlayerType.WARRIOR;
    [SerializeField] GameObject playerProjectile;

    // Start is called before the first frame update
    void Start()
    {
        this.health = 3;
        this.projectileCooldown = 0.5f;
        this.projectileReloading = false;
        this.projectileDuration = 0.0f;

        // set player sprite/animations
        var animator = gameObject.GetComponent<Animator>();
        switch(playerType)
        {
            case PlayerType.WARRIOR:
                // set Animation Controller
                animator.runtimeAnimatorController
                = Resources.Load<RuntimeAnimatorController>("Sprites/PlayerSprites/Animations/Warrior_Animations/AC_Warrior");
                break;
            case PlayerType.SORCERESS:
                // set Animation Controller
                animator.runtimeAnimatorController
                = Resources.Load<RuntimeAnimatorController>("Sprites/PlayerSprites/Animations/Sorceress_Animations/AC_Sorceress");
                break;
            default:
                Debug.Log("Error: player type is undefined.");
                break;
        }
    }

    public void IncreaseHealth(int amount)
    {
        this.health += amount;
        Debug.Log(this.health);
    }

    public void DecreaseHealth(int amount)
    {
        this.health -= amount;
        Debug.Log(this.health);
    }

    public int GetHealth()
    {
        return this.health;
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
