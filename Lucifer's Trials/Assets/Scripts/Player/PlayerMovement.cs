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
    private bool dashlocked = false;
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
            FindObjectOfType<SoundManager>().PlaySoundEffect("Dash");
            this.GetComponent<PlayerAnimationController>().PlayDashEffect();
            this.isDashing = true;
            this.curDuration = 0.0f;
            this.sinceDash = 0.0f;
            // ensure all attack sprites are disabled (no damaging enemies while dashing)
            for (int i = 0; i < 4; i++)
            {
                // all attack sprites are the first 4 gameobjects of the player
                this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        if (this.dashlocked && (this.curDuration >= this.dashDuration))
        {
            this.dashlocked = false;
            this.isDashing = false;
        }
    }

    void FixedUpdate()
    {
        bool statelock = this.GetComponent<PlayerAnimationController>().GetStateLock();
        if (!statelock && !dashlocked)
        {
            if (this.isDashing)
            {
                // When dashing, up speed in movement direction
                this.dashlocked = true;
                this.speed = GameObject.Find("Player").GetComponent<PlayerController>().GetSpeed();
                this.rb.velocity = new Vector2(this.horizontal, this.vertical).normalized * (this.speed * 3f);
            }
            else
            {
                this.speed = GameObject.Find("Player").GetComponent<PlayerController>().GetSpeed();
                this.isDashing = false;
                this.rb.velocity = new Vector2(this.horizontal, this.vertical).normalized * this.speed;
            }
        }
        else
        {
            // Dash duration handling
            this.curDuration += Time.deltaTime;
        }
    }
}
