using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Lucifer;

public class PlayerMovement : MonoBehaviour
{
    private float speed;
    private float knockbackForce = 4.5f;
    private float horizontal;
    private float vertical;
    private bool isDashing = false;
    private bool movelocked = false;
    private float dashDuration = 0.1f;
    private float dashCooldown = 0.6f;
    private float sinceDash = 1.5f;
    private float curDuration;
    private string facedDirection = "Down";
    Action DeactivateAttackSprite;
    Func<bool> GetStateLock;
    Func<int> GetSpeed;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        DeactivateAttackSprite = this.gameObject.GetComponent<PlayerAttackController>().DeactivateAttackSprite;
        GetStateLock = this.GetComponent<PlayerAnimationController>().GetStateLock;
        GetSpeed = GameObject.Find("Player").GetComponent<PlayerController>().GetSpeed;
    }

    public bool GetMoveLocked()
    {
        return this.movelocked;
    }

    public string GetFacedDirection()
    {
        return this.facedDirection;
    }

    public bool IsDashing()
    {
        return this.isDashing;
    }

    public void Knockback(Vector2 direction)
    {
        this.rb.velocity = direction * (new Vector2(knockbackForce, knockbackForce));
    }

    // Update is called once per frame
    void Update()
    {
        this.sinceDash += Time.deltaTime;

        // attack input
        if (Input.GetButtonDown("Fire1") && !this.isDashing)
        {
            this.rb.velocity = Vector2.zero;
        }

        // NOTE: Deadzone for dash is defined here statically
        // Pressed space, player is idling/walking, outside of deadzone, and dash not in cooldown
        if (Input.GetKeyDown("space") && !GetStateLock() && (this.rb.velocity.magnitude >= 0.1f) && (this.sinceDash >= this.dashCooldown))
        {
            // Ensure attack sprite is never active during attack interruption
            DeactivateAttackSprite();
            this.isDashing = true;
            this.curDuration = 0.0f;
            this.sinceDash = 0.0f;
        }
        
        // Amidst dash and dash cooldown has finished
        if (this.movelocked && (this.curDuration >= this.dashDuration))
        {
            this.movelocked = false;
            this.isDashing = false;
        }
    }

    void FixedUpdate()
    {
        this.horizontal = Input.GetAxisRaw("Horizontal");
        this.vertical = Input.GetAxisRaw("Vertical");

        // Player is idling/walking and is not amidst a dash
        if (!GetStateLock() && !movelocked)
        {
            if (this.isDashing)
            {
                // If a dash has just begun
                if (!this.movelocked)
                {
                    this.GetComponent<PlayerAnimationController>().PlayDashEffect();
                }
                // Dashing := speed up in movement direction
                this.movelocked = true;
                this.rb.velocity = this.rb.velocity.normalized * (GetSpeed() * 3f);
            }
            else
            {
                this.isDashing = false;
                this.rb.velocity = new Vector2(this.horizontal, this.vertical).normalized * GetSpeed();
            }
        }
        else
        {
            // Dash duration handling
            this.curDuration += Time.deltaTime;
        }
    }
}
