using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimation))]
public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    private EnemyTypes enemyType;
    private GameObject attackSprite;
    private enum Sprite { DOWN, LEFT, RIGHT, UP }

    public void Start()
    {
        enemyType = this.gameObject.GetComponent<EnemyAnimation>().GetEnemyType();
    }

    public void ActivateAttackSprite()
    {
        var animator = this.gameObject.GetComponent<Animator>();
        float x = animator.GetFloat("MoveX");
        float y = animator.GetFloat("MoveY");
        Vector2 enemyVec = new Vector2(x, y);
        float minDist;
        int option;

        /// Determine closest cardinal direction
        // default to down
        minDist = Vector2.Distance(Vector2.down, enemyVec);
        option = 1;
        // relative to left
        var distanceEval = Vector2.Distance(Vector2.left, enemyVec);
        if (distanceEval < minDist)
        {
            minDist = distanceEval;
            option = 2;
        }
        // relative to right
        distanceEval = Vector2.Distance(Vector2.right, enemyVec);
        if (distanceEval < minDist)
        {
            minDist = distanceEval;
            option = 3;
        }
        // relative to up
        distanceEval = Vector2.Distance(Vector2.up, enemyVec);
        if (distanceEval < minDist)
        {
            minDist = distanceEval;
            option = 4;
        }

        // activate properly oriented sprite
        switch (option)
        {
            case 1:
                switch (enemyType)
                {
                    case EnemyTypes.BESERKER:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.DOWN).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case EnemyTypes.SLINGER:
                        var projectile = (GameObject)Instantiate(this.projectilePrefab, transform.position + (new Vector3(-0.1f, 0, 0)), Quaternion.Euler(0, 0, 135));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
                break;
            case 2:
                switch (enemyType)
                {
                    case EnemyTypes.BESERKER:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.LEFT).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case EnemyTypes.SLINGER:
                        var projectile = (GameObject)Instantiate(this.projectilePrefab, transform.position + (new Vector3(0, -0.1f, 0)), Quaternion.Euler(0, 0, 45));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
                break;
            case 3:
                switch (enemyType)
                {
                    case EnemyTypes.BESERKER:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.RIGHT).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case EnemyTypes.SLINGER:
                        var projectile = (GameObject)Instantiate(this.projectilePrefab, transform.position + (new Vector3(0, -0.1f, 0)), Quaternion.Euler(0, 0, 225));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
                break;
            case 4:
                switch (enemyType)
                {
                    case EnemyTypes.BESERKER:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.UP).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case EnemyTypes.SLINGER:
                        var projectile = (GameObject)Instantiate(this.projectilePrefab, transform.position + (new Vector3(0.1f, 0, 0)), Quaternion.Euler(0, 0, -45));
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                        break;
                }
                break;
            default:
                Debug.Log("Error: Invalid direction provided.");
                break;
        }

    }

    public void DeactivateAttackSprite()
    {
        attackSprite.SetActive(false);
    }

}