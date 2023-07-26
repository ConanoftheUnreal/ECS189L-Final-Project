using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoblinBeserker))]
[RequireComponent(typeof(EnemyAnimation))]
public class EnemyMovement : MonoBehaviour
{
    Func<bool> GetStateLock;
    private bool statelock = false;
    private bool cornerAdjustNeeded = false;
    private bool pausedMovement = false;
    private float knockbackForce = 2f;
    private float timePassed = 0.0f;
    private Vector2 lastDirection;
    private Rigidbody2D rb;
    private GoblinBeserker enemy;

    public void Start()
    {
        this.rb = this.gameObject.GetComponent<Rigidbody2D>();
        this.enemy = this.gameObject.GetComponent<GoblinBeserker>();

        this.GetStateLock = this.gameObject.GetComponent<EnemyAnimation>().GetStateLock;
    }

    public void Knockback(Vector2 direction, int damage)
    {
        this.rb.velocity = direction * (new Vector2(knockbackForce * Math.Min(damage, 3.0f), knockbackForce * Math.Min(damage, 3.0f)));
    }

    private void AdjustDirection()
    {
        // For fixing stuck-on-upper-corner issue
        this.rb.velocity = Vector2.up * this.enemy.Speed;
    }

    public void FixedUpdate()
    {
        statelock = GetStateLock();
        if (!statelock)
        {
            if (this.pausedMovement)
            {
                this.rb.velocity = Vector2.zero;
                this.timePassed += Time.deltaTime;
                if (this.timePassed >= 0.2f)
                {
                    this.timePassed = 0.0f;
                    this.pausedMovement = false;
                }
            }
            else if (this.cornerAdjustNeeded)
            {
                AdjustDirection();
                // After certain time of adjusting, Enemy should have surpassed corner
                if (this.timePassed >= 0.05)
                {
                    this.cornerAdjustNeeded = false;
                }
                this.timePassed += Time.deltaTime;
            }
            else
            {
                // Add time passage when velocity is below desired range (perhaps stuck)
                if (this.rb.velocity.magnitude <= 0.1f)
                {
                    this.timePassed += Time.deltaTime;
                }
                else
                {
                    this.timePassed = 0.0f;
                }
                
                // After a certain time of very low speed, adjust direction (assume stuck)
                if (this.timePassed >= 0.05)
                {
                    this.timePassed = 0.0f;
                    this.cornerAdjustNeeded = true;
                }
                else
                {
                    // If drastic alteration of desired direction, enemy pauses (decreases jittering)
                    if (Vector2.Angle(this.lastDirection, this.enemy.MovementDirection) >= 90)
                    {
                        this.timePassed = 0.0f;
                        this.pausedMovement = true;
                    }
                    this.rb.velocity = this.enemy.MovementDirection.normalized * this.enemy.Speed;
                    this.lastDirection = this.enemy.MovementDirection;
                }
            }
        }
    }
}