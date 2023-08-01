using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] GameObject playerProjectile;
    GameObject attackSprite;
    private enum Sprite { DOWN, LEFT, RIGHT, UP }

    private bool canAttack = true;
    private float timePassed = 0.0f;
    private GameObject currentProjectile;

    public void ActivateAttackSprite()
    {
        if (!this.canAttack) return;
        
        this.canAttack = false;

        var playerClass = this.GetComponent<PlayerAnimationController>().GetPlayerType();
        var animator = this.gameObject.GetComponent<Animator>();
        float x = animator.GetFloat("MoveX");
        float y = animator.GetFloat("MoveY");

        var projectileDamage = 0;
        GameObject projectile = null;

        // right or left
        if (x != 0.0f)
        {
            if (x > 0.0f)
            {
                switch (playerClass)
                {
                    case PlayerType.WARRIOR:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.RIGHT).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case PlayerType.SORCERESS:
                        projectileDamage = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();
                        playerProjectile.GetComponent<ProjectileScript>().SetProjectileDamage(projectileDamage);
                        projectile = (GameObject)Instantiate(playerProjectile, transform.position + (new Vector3(0.5f, -0.1f, 0)), Quaternion.Euler(0, 0, 180));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
            }
            else
            {
                switch (playerClass)
                {
                    case PlayerType.WARRIOR:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.LEFT).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case PlayerType.SORCERESS:
                        projectileDamage = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();
                        playerProjectile.GetComponent<ProjectileScript>().SetProjectileDamage(projectileDamage);
                        projectile = (GameObject)Instantiate(playerProjectile, transform.position + (new Vector3(-0.5f, -0.1f, 0)), Quaternion.Euler(0, 0, 0));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
            }
        }
        // up or down
        else
        {
            if (y > 0.0f)
            {
                switch (playerClass)
                {
                    case PlayerType.WARRIOR:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.UP).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case PlayerType.SORCERESS:
                        projectileDamage = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();
                        playerProjectile.GetComponent<ProjectileScript>().SetProjectileDamage(projectileDamage);
                        projectile = (GameObject)Instantiate(playerProjectile, transform.position + (new Vector3(0.1f, 0.5f, 0)), Quaternion.Euler(0, 0, -90));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
            }
            else
            {
                switch (playerClass)
                {
                    case PlayerType.WARRIOR:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.DOWN).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case PlayerType.SORCERESS:
                        projectileDamage = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();
                        playerProjectile.GetComponent<ProjectileScript>().SetProjectileDamage(projectileDamage);
                        projectile = (GameObject)Instantiate(playerProjectile, transform.position + (new Vector3(-0.1f, -0.5f, 0)), Quaternion.Euler(0, 0, 90));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
            }
        }

        if (projectile != null)
        {
            this.currentProjectile = projectile;
            Destroy(this.currentProjectile, 0.5f);
        }
    }

    public void DeactivateAttackSprite()
    {
        if (attackSprite != null)
        {
            attackSprite.SetActive(false);
        }
    }

    public void Update()
    {
        // fix for double attack consequence due to blend tree for animations
        if (this.timePassed >= 0.1f)
        {
            this.timePassed = 0.0f;
            this.canAttack = true;
        }
        else
        {
            this.timePassed += Time.deltaTime;
        }

        // dissipate fireball into nothing (decreases usable distance)
        if (this.currentProjectile != null)
        {
            var scaleVec = this.currentProjectile.transform.localScale;
            this.currentProjectile.transform.localScale = scaleVec - (new Vector3(Time.deltaTime * 1.75f, Time.deltaTime * 1.75f, 0f));
        }
    }

}