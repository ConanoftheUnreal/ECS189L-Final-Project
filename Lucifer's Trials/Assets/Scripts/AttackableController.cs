using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class AttackableController : MonoBehaviour
{
    private bool knockedback = false;
    private float knockbackDuration = 0.25f;
    private float timePassed;
    private float knockbackForce = 3.0f;
    [SerializeField] int damage = 1;

    private int hitpoints = 5;
    private int maxHitpoints = 5;
    [SerializeField] private HealthBarController healthBar;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerHurtbox")
        {
            // get `Player` gameobject from collider of `PlayerHurtbox` and hurt player
            col.transform.parent.GetComponent<PlayerController>().DecreaseHealth(damage);
            // notify player they were damaged
            col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.CQC);
        }

        if ((col.tag == "PlayerAttack") || (col.tag == "Projectile"))
        {
            this.TakeDamage();
            FindObjectOfType<SoundManager>().PlaySoundEffect("enemy hurt");
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

    void Start()
    {
        this.healthBar.SetHealth(this.hitpoints, this.maxHitpoints);
    }

    private void TakeDamage()
    {
        if (this.hitpoints > 0)
        {
            this.hitpoints -= 1;
            this.healthBar.SetHealth(this.hitpoints, this.maxHitpoints);
        }
    }

    // Update is called once per frame
    void Update()
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
