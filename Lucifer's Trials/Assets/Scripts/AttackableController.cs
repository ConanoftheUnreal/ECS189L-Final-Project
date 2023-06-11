using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class AttackableController : MonoBehaviour
{
    // Attack interaction data
    [SerializeField] GameObject deathEffect;
    [SerializeField] bool humanoid = false;
    private bool knockedback = false;
    private float knockbackDuration = 0.25f;
    private float timePassed;
    private float knockbackForce = 4.5f;
    private Rigidbody2D rb;

    // Healthbar data
    private int hitpoints;
    private HealthBarController healthBar;

    // enemy data
    private GoblinBeserker self;
    private int damage;
    private int maxHitpoints;

    public void Start()
    {
        this.healthBar = this.gameObject.transform.Find("Healthbar").gameObject.GetComponent<HealthBarController>();

        this.self = this.gameObject.GetComponent<GoblinBeserker>();
        if (this.self)
        {
            // determined by stats of the enemy
            this.maxHitpoints = (int)this.self.Stats.Health;
        }
        else
        {
            // default for a random object
            this.maxHitpoints = 3;
        }
        this.hitpoints = this.maxHitpoints;

        this.rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        // if collision with player, hurt the player
        if (col.tag == "PlayerHurtbox")
        {
            if (hitpoints > 0)
            {
                FindObjectOfType<SoundManager>().PlaySoundEffect("Player Hurt");
                col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.COLLIDE);
            }
        }

        // if collision with something that hurts, self gets hurt
        if ((col.tag == "PlayerAttack") || (col.tag == "Projectile"))
        {
            // get damage based on type of attacking object; NOT YET IMPLEMENTED, VALUE IS A PLACEHOLDER VALUE
            var attackersDamage = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();

            var wasProjectile = false;
            if (col.tag == "Projectile")
            {
                wasProjectile = true;
            }

            FindObjectOfType<SoundManager>().PlaySoundEffect("Enemy Hurt");
            this.TakeDamage(col, attackersDamage, wasProjectile);
        }
    }

    private void TakeDamage(Collider2D col, int attackersDamage, bool wasProjectile)
    {
        // attacking enemy
        if (humanoid)
        {
            var killed = false;
            this.hitpoints -= attackersDamage;
            if (hitpoints <= 0)
            {
                killed = true;
            }
            this.gameObject.GetComponent<EnemyAnimation>().EnemyDamaged(col.gameObject, killed, wasProjectile);
        }
        // attacking breakable object
        else
        {
            this.hitpoints -= attackersDamage;
            if (this.hitpoints <= 0)
            {
                DeathDisappear();
            }

            var positionSelf = (Vector2)this.gameObject.transform.position;
            Vector2 positionAttack;
            if (col.tag == "PlayerAttack")
            {
                positionAttack = col.ClosestPoint(col.transform.parent.transform.position);
            }
            else
            {
                positionAttack = col.ClosestPoint(this.gameObject.transform.position);
            }

            var knockbackDirection = (positionSelf - positionAttack).normalized;

            this.rb.velocity = knockbackForce * knockbackDirection;
            knockedback = true;
            timePassed = 0.0f;
        }
    }

    // called upon death animation complete or object destroyed
    public void DeathDisappear()
    {
        var effect = Instantiate(this.deathEffect, this.transform.position, Quaternion.identity) as GameObject;
        Destroy(this.gameObject);
    }

    public void Update()
    {
        this.healthBar.SetHealth(this.hitpoints, this.maxHitpoints);
        if (knockedback)
        {
            if (timePassed >= knockbackDuration)
            {
                this.rb.velocity = Vector2.zero;
                knockedback = false;
            }
            timePassed += Time.deltaTime;
        }
    }
}
