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
    private bool canBeHit = true;
    private float knockbackDuration = 0.25f;
    private float sinceKnockback;
    private float sinceHit;
    private float knockbackForce = 4.5f;
    private Rigidbody2D rb;

    // Healthbar data
    private int hitpoints;
    private HealthBarController healthBar;

    // enemy data
    private GoblinBeserker self;
    private int damage;
    private int maxHitpoints;

    // ensures enemy only drops one item
    private bool alreadyDropped = false;

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

        // bump damage
        this.damage = 1;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!canBeHit) return;

        canBeHit = false;

        // if collision with player, hurt the player
        if (col.tag == "PlayerHurtbox")
        {
            if (hitpoints > 0)
            {
                var wasHurt = col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.COLLIDE);
                if (wasHurt)
                {
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Player Hurt");
                }
            }
        }

        // if collision with something that hurts, self gets hurt
        if (((col.tag == "PlayerAttack") || (col.tag == "Projectile")))
        {
            var attackersDamage = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();

            var wasProjectile = false;
            if (col.tag == "Projectile")
            {
                wasProjectile = true;
            }

            if (this.hitpoints > 0)
            {
                FindObjectOfType<SoundManager>().PlaySoundEffect("Enemy Hurt");
            }
            this.TakeDamage(col, attackersDamage, wasProjectile);
        }
    }

    private void TakeDamage(Collider2D col, int attackersDamage, bool wasProjectile)
    {
        // attacking enemy
        if (humanoid)
        {
            // shouldn't be able to take damage if already at or below 0 health
            if (this.hitpoints <= 0)
            {
                return;
            }
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
            sinceKnockback = 0.0f;
        }
    }

    // called upon death animation complete or object destroyed
    public void DeathDisappear()
    {
        var effect = Instantiate(this.deathEffect, this.transform.position, Quaternion.identity) as GameObject;
        if (!this.alreadyDropped)
        {
            var drop = this.gameObject.GetComponent<DropRateLogic>().GetEnemyDrop();
            if (drop != null)
            {
                Instantiate(drop, this.transform.position - (new Vector3(0, 0.3f, 0)), Quaternion.identity);
            }
            this.alreadyDropped = true;
        }
        Destroy(this.gameObject);
    }

    public void Update()
    {
        this.healthBar.SetHealth(this.hitpoints, this.maxHitpoints);
        if (this.knockedback)
        {
            if (this.sinceKnockback >= knockbackDuration)
            {
                this.rb.velocity = Vector2.zero;
                this.knockedback = false;
            }
            this.sinceKnockback += Time.deltaTime;
        }

        // fix for double hitting enemies/objects
        if (this.sinceHit >= 0.05f)
        {
            this.sinceHit = 0.0f;
            this.canBeHit = true;
        }
        else
        {
            this.sinceHit += Time.deltaTime;
        }
    }
}
