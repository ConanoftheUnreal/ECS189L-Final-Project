using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 8.0f;
    private float horizontal;
    private float vertical;
    private bool isDashing = false;
    private float dashDuration = 0.25f;
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
        if (Input.GetKeyDown("space") && rb.velocity != Vector2.zero)
        {
            Debug.Log("Space Pressed");
            isDashing = true;
            curDuration = 0.0f;
        }

        if (!isDashing)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (horizontal == -1 && vertical == 0)
            {
                //Debug.Log("Facing Left");
                facedDirection = "Left";
            }
            if (horizontal == 1 && vertical == 0)
            {
                //Debug.Log("Facing Right");
                facedDirection = "Right";
            }
            if (horizontal == 0 && vertical == 1)
            {
                //Debug.Log("Facing Up");
                facedDirection = "Up";
            }
            if (horizontal == 0 && vertical == -1)
            {
                //Debug.Log("Facing Down");
                facedDirection = "Down";
            }

            Debug.Log(facedDirection);
        }
    }

    void FixedUpdate()
    {
        if (isDashing && curDuration < dashDuration)
        {
            Debug.Log("Dashing");
            rb.velocity = new Vector2(horizontal, vertical).normalized * (speed * 2f);
            curDuration += Time.deltaTime;
        }
        else
        {
            isDashing = false;
            rb.velocity = new Vector2(horizontal, vertical).normalized * speed;
        }
    }
}
