using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] GameObject playerProjectile;
    GameObject attackSprite;
    private enum Sprite { DOWN, LEFT, RIGHT, UP }

    public void ActivateAttackSprite()
    {
        var playerClass = this.GetComponent<PlayerAnimationController>().GetPlayerType();
        var animator = this.gameObject.GetComponent<Animator>();
        // division is for placement of projectiles
        float x = animator.GetFloat("MoveX") / 2;
        float y = animator.GetFloat("MoveY") / 2;

        // right or left
        if (x != 0)
        {
            if (x > 0)
            {
                switch (playerClass)
                {
                    case PlayerType.WARRIOR:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.RIGHT).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case PlayerType.SORCERESS:
                        var projectile = (GameObject)Instantiate(playerProjectile, transform.position + (new Vector3(x, -0.1f, 0)), Quaternion.Euler(0, 0, 180));
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
                        var projectile = (GameObject)Instantiate(playerProjectile, transform.position + (new Vector3(x, -0.1f, 0)), Quaternion.Euler(0, 0, 0));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
            }
        }
        // up or down
        else
        {
            if (y > 0)
            {
                switch (playerClass)
                {
                    case PlayerType.WARRIOR:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.UP).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case PlayerType.SORCERESS:
                        var projectile = (GameObject)Instantiate(playerProjectile, transform.position + (new Vector3(0.1f, y, 0)), Quaternion.Euler(0, 0, -90));
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
                        var projectile = (GameObject)Instantiate(playerProjectile, transform.position + (new Vector3(-0.1f, y, 0)), Quaternion.Euler(0, 0, 90));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
            }
        }
    }

    public void DeactivateAttackSprite()
    {
        attackSprite.SetActive(false);
    }

}