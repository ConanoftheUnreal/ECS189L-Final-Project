using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Lucifer;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 4.0f;
    private float knockbackForce = 3.0f;
    private float horizontal;
    private float vertical;
    private bool isDashing = false;
    private float dashDuration = 0.1f;
    private float dashCooldown = 0.6f;
    private float sinceDash = 1.5f;
    private float curDuration;
    private string facedDirection = "Down";
    [SerializeField] private Rigidbody2D rb;

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
        var rbPlayer = this.GetComponent<Rigidbody2D>();
        rbPlayer.velocity = direction * (new Vector2(knockbackForce, knockbackForce));
    }

    // Update is called once per frame
    void Update()
    {
        this.horizontal = Input.GetAxisRaw("Horizontal");
        this.vertical = Input.GetAxisRaw("Vertical");

        this.sinceDash += Time.deltaTime;

        // attack input
        if (Input.GetButtonDown("Fire1"))
        {
            this.rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown("space") && (this.rb.velocity != Vector2.zero) && (this.sinceDash >= this.dashCooldown))
        {
            // Debug.Log("Space Pressed");
            FindObjectOfType<SoundManager>().PlaySoundEffect("Dash");
            this.isDashing = true;
            this.curDuration = 0.0f;
            this.sinceDash = 0.0f;
        }

        if (!this.isDashing)
        {
            if (this.horizontal == -1)
            {
                this.facedDirection = "Left";
            }
            if (this.horizontal == 1)
            {
                this.facedDirection = "Right";
            }
            if (this.horizontal == 0 && this.vertical == 1)
            {
                this.facedDirection = "Up";
            }
            if (this.horizontal == 0 && this.vertical == -1)
            {
                this.facedDirection = "Down";
            }
        }
        
    }

    void FixedUpdate()
    {
        bool statelock = this.GetComponent<PlayerAnimationController>().GetStateLock();
        if (!statelock)
        {
            if (this.isDashing && (this.curDuration < this.dashDuration))
            {
                this.rb.velocity = new Vector2(this.horizontal, this.vertical).normalized * (this.speed * 3f);
                this.curDuration += Time.deltaTime;
            }
            else
            {
                this.isDashing = false;
                this.rb.velocity = new Vector2(this.horizontal, this.vertical).normalized * this.speed;
            }
        }
    }
}
