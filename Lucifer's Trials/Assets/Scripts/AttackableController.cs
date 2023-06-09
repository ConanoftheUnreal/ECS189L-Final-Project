using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class AttackableController : MonoBehaviour
{
    // Attack interaction data
    [SerializeField] GameObject deathEffect;
    [SerializeField] int damage = 1;
    private bool knockedback = false;
    private float knockbackDuration = 0.25f;
    private float timePassed;
    private float knockbackForce = 3.0f;
    // Healthbar data
    [SerializeField] private HealthBarController healthBar;
    [SerializeField] int maxHitpoints;
    private int hitpoints;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerHurtbox")
        {
            col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.CQC);
        }

        if ((col.tag == "PlayerAttack") || (col.tag == "Projectile"))
        {
            this.TakeDamage();
            FindObjectOfType<SoundManager>().PlaySoundEffect("EnemyHurt");
            var rb = this.GetComponent<Rigidbody2D>();

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

            rb.velocity = knockbackForce * knockbackDirection;
            knockedback = true;
            timePassed = 0.0f;
        }
    }

    public void Start()
    {
        this.hitpoints = this.maxHitpoints;
        this.healthBar.SetHealth(this.hitpoints, this.maxHitpoints);
    }

    private void TakeDamage()
    {
        this.hitpoints -= 1;
        if (this.hitpoints > 0)
        {
            this.healthBar.SetHealth(this.hitpoints, this.maxHitpoints);
        }
        else
        {
            // queue death animation of object
            // TEMPORARY FOR TESTING:
            DeathDisappear();
        }
    }

    public void DeathDisappear()
    {
        var effect = Instantiate(this.deathEffect, this.transform.position, Quaternion.identity) as GameObject;
        Destroy(this.gameObject);
    }

    public void Update()
    {
        if (knockedback)
        {
            if (timePassed >= knockbackDuration)
            {
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                knockedback = false;
            }
            timePassed += Time.deltaTime;
        }
    }
}
