using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 8.0f;
    private float horizontal;
    private float vertical;
    private bool isDashing = false;
    private float dashDuration = 0.25f;
    private float curDuration;

    [SerializeField] private Rigidbody2D rb;

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
        }
    }

    void FixedUpdate()
    {
        if (isDashing && curDuration < dashDuration)
        {
            Debug.Log("Dashing");
            rb.velocity = new Vector2(horizontal, vertical).normalized * (speed * 1.5f);
            curDuration += Time.deltaTime;
        }
        else
        {
            isDashing = false;
            rb.velocity = new Vector2(horizontal, vertical).normalized * speed;
        }
    }
}
