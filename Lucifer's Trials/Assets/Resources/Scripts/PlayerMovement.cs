using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private bool statelock = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Rigidbody2D rb;

    private enum PlayerStates { IDLE, WALK, ATTACK }
    private PlayerStates currentState;
    private PlayerStates CurrentState
    {
        set
        {
            if (!statelock)
            {
                currentState = value;

                // set which animation the sprite uses
                switch(currentState)
                {
                    case PlayerStates.IDLE:
                        animator.Play("Idle");
                        break;
                    case PlayerStates.WALK:
                        animator.Play("Walk");
                        break;
                    case PlayerStates.ATTACK:
                        animator.Play("Attack1");
                        statelock = true;
                        break;
                    default:
                        Debug.Log("Error: player state is undefined.");
                        break;
                }
            }
        }
    }

    public string GetFacedDirection()
    {
        return this.facedDirection;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void attackFinished()
    {
        statelock = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        sinceDash += Time.deltaTime;

        // attack input
        if (Input.GetKeyDown(KeyCode.B))
        {
            CurrentState = PlayerStates.ATTACK;
            animator.speed = speed;
            rb.velocity = Vector2.zero;
        }
        // player movement
        else if ( ((horizontal != 0) || (vertical != 0)) && (!statelock) )
        {
            CurrentState = PlayerStates.WALK;
            animator.speed = speed / 2;
            animator.SetFloat("MoveX", horizontal);
            animator.SetFloat("MoveY", vertical);
        }
        else
        {
            CurrentState = PlayerStates.IDLE;
            animator.speed = 1;
        }

        if (Input.GetKeyDown("space") && (rb.velocity != Vector2.zero) && (sinceDash >= dashCooldown))
        {
            // Debug.Log("Space Pressed");
            isDashing = true;
            curDuration = 0.0f;
            sinceDash = 0.0f;
        }

        if (!isDashing)
        {
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

            // Debug.Log(facedDirection);
        }
    }

    void FixedUpdate()
    {
        if (!statelock)
        {
            if (isDashing && (curDuration < dashDuration))
            {
                // Debug.Log("Dashing");
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
}
