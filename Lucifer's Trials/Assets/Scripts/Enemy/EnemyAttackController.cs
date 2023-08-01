using System;
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

    private bool canActivate;
    private float timeSince;

    public void Start()
    {
        this.enemyType = this.gameObject.GetComponent<EnemyAnimation>().GetEnemyType();

        this.canActivate = true;
        this.timeSince = 0.0f;
    }

    private Vector2 EnemyInaccuracy(float magnitude)
    {
        // Could return a different amount dependent upon levels cleared, times player leveled up, etc.
        // 0 = right on target, 1 = inconsistent aim
        var badAim = 1.0f;  // currently static
        // Aiming is easier for enemy if player is close
        var proximityEase = Math.Min(1.0f, magnitude / 3.0f);
        return new Vector2(proximityEase * UnityEngine.Random.Range(-badAim, badAim), proximityEase * UnityEngine.Random.Range(-badAim, badAim));
    }

    public void ActivateAttackSprite()
    {
        if (!canActivate) return;

        canActivate = false;
        
        // Enemy grows when attacking
        FindObjectOfType<SoundManager>().PlaySoundEffect("Enemy Growl");

        var animator = this.gameObject.GetComponent<Animator>();
        float x = animator.GetFloat("MoveX");
        float y = animator.GetFloat("MoveY");
        Vector2 enemyVec = new Vector2(x, y);

        switch (enemyType)
        {
            case EnemyTypes.BESERKER:

                float minDist;
                int option;

                /// Determine closest cardinal direction
                // Default to down
                minDist = Vector2.Distance(Vector2.down, enemyVec);
                option = 1;
                // Relative to left
                var distanceEval = Vector2.Distance(Vector2.left, enemyVec);
                if (distanceEval < minDist)
                {
                    minDist = distanceEval;
                    option = 2;
                }
                // Relative to right
                distanceEval = Vector2.Distance(Vector2.right, enemyVec);
                if (distanceEval < minDist)
                {
                    minDist = distanceEval;
                    option = 3;
                }
                // Relative to up
                distanceEval = Vector2.Distance(Vector2.up, enemyVec);
                if (distanceEval < minDist)
                {
                    minDist = distanceEval;
                    option = 4;
                }

                // Activate properly oriented sprite
                switch (option)
                {
                    case 1:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.DOWN).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case 2:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.LEFT).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case 3:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.RIGHT).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    case 4:
                        attackSprite = this.gameObject.transform.GetChild((int)Sprite.UP).gameObject;
                        attackSprite.SetActive(true);
                        break;
                    default:
                        Debug.Log("Error: Invalid direction provided.");
                        break;
                }
                break;
            
            case EnemyTypes.SLINGER:

                // Shoot an arrow in the direction of the player with random adjustment
                var directionVec = (Vector2)(GameObject.Find("Player").transform.position - this.gameObject.transform.position);
                directionVec = directionVec + EnemyInaccuracy(directionVec.magnitude);
                // Readjust orientation of enemy if player has moved around it
                if (Vector2.Angle(directionVec, enemyVec) >= 90)
                {
                    animator.SetFloat("MoveX", directionVec.x * 2);
                    animator.SetFloat("MoveY", directionVec.y * 2);
                }

                // Start sprite pointing right
                float theta = -135;
                // Determine proper sprite orientation along velocity vector
                theta = theta + Vector2.SignedAngle(Vector2.right, directionVec);

                // Instantiate arrow and its attributes based on previous determinants
                var projectile = (GameObject)Instantiate(this.projectilePrefab, transform.position, Quaternion.Euler(0, 0, theta));
                projectile.GetComponent<Rigidbody2D>().velocity = directionVec.normalized * projectile.GetComponent<ProjectileScript>().GetProjectileSpeed();
                break;
            
            default:
                Debug.Log("Error: Invalid enemy type.");
                break;
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
        if (!canActivate)
        {
            timeSince += Time.deltaTime;
            if (timeSince >= 0.1)
            {
                canActivate = true;
                timeSince = 0.0f;
            }
        }
    }

}