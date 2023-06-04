using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Lucifer;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 4.0f;
    private float horizontal;
    private float vertical;
    private bool isDashing = false;
    private float dashDuration = 0.25f;
    private float dashCooldown = 0.6f;
    private float sinceDash = 1.5f;
    private float curDuration;
    private string facedDirection = "Down";
    [SerializeField] private Rigidbody2D rb;

    public string GetFacedDirection()
    {
        return this.facedDirection;
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
            this.isDashing = true;
            this.curDuration = 0.0f;
            this.sinceDash = 0.0f;
        }

        if (!this.isDashing)
        {
            if (this.horizontal == -1 && this.vertical == 0)
            {
                //Debug.Log("Facing Left");
                this.facedDirection = "Left";
            }
            if (this.horizontal == 1 && this.vertical == 0)
            {
                //Debug.Log("Facing Right");
                this.facedDirection = "Right";
            }
            if (this.horizontal == 0 && this.vertical == 1)
            {
                //Debug.Log("Facing Up");
                this.facedDirection = "Up";
            }
            if (this.horizontal == 0 && this.vertical == -1)
            {
                //Debug.Log("Facing Down");
                this.facedDirection = "Down";
            }

            // Debug.Log(facedDirection);
        }
    }

    void FixedUpdate()
    {
        bool statelock = this.GetComponent<PlayerAnimationController>().GetStateLock();
        if (!statelock)
        {
            if (this.isDashing && (this.curDuration < this.dashDuration))
            {
                // Debug.Log("Dashing");
                this.rb.velocity = new Vector2(this.horizontal, this.vertical).normalized * (this.speed * 2f);
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
