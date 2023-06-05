using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType = PlayerType.WARRIOR;

    private bool statelock = false;
    private bool playerHurt = false;
    private float speed = 4.0f;
    private float horizontal;
    private float vertical;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerStates currentState;
    private PlayerStates CurrentState
    {
        set
        {
            if (!this.statelock || this.playerHurt)
            {
                this.currentState = value;

                // set which animation the sprite uses
                switch (this.currentState)
                {
                    case PlayerStates.IDLE:
                        this.animator.Play("Idle");
                        break;
                    case PlayerStates.WALK:
                        this.animator.Play("Walk");
                        break;
                    case PlayerStates.ATTACK:
                        this.animator.Play("Attack1");
                        this.statelock = true;
                        break;
                    case PlayerStates.HURT:
                        this.animator.Play("Hurt");
                        this.statelock = true;
                        break;
                    default:
                        Debug.Log("Error: player state is undefined.");
                        break;
                }
            }
        }
    }

    public void AttackFinished()
    {
        this.statelock = false;
    }

    public void HurtFinished()
    {
        this.statelock = false;
        this.playerHurt = false;
    }

    public bool GetStateLock()
    {
        return this.statelock;
    }

    public PlayerType GetPlayerType()
    {
        return this.playerType;
    }

    public void PlayerDamaged(GameObject obj, int damage, DamageTypes damageType)
    {
        // determine location of collision relative to player
        var collisionPt = obj.GetComponent<Collider2D>().ClosestPoint(this.gameObject.transform.position);
        var knockbackDirection = ((Vector2)this.gameObject.transform.position - collisionPt).normalized;
        
        // queue player hurt
        this.playerHurt = true;
        this.CurrentState = PlayerStates.HURT;
        this.animator.speed = 1;

        // determine knockback handling
        Action<Vector2> Knockback = this.gameObject.GetComponent<PlayerMovement>().Knockback;
        switch (damageType)
        {
            case DamageTypes.CQC:
                // determine knockback by vector btw both objects
                this.animator.SetFloat("MoveX", -knockbackDirection.x);
                this.animator.SetFloat("MoveY", -knockbackDirection.y);
                Knockback(knockbackDirection);
                break;
            case DamageTypes.RANGED:
                // determine knockback by direction of missile
                var objVec = obj.GetComponent<Rigidbody2D>().velocity.normalized;
                this.animator.SetFloat("MoveX", -objVec.x);
                this.animator.SetFloat("MoveY", -objVec.y);
                Knockback(objVec);
                break;
            case DamageTypes.AOE:
                // unimplmented; maybe we don't want knockback for AOE
                break;
            default:
                Debug.Log("Error: damage type is undefined.");
                break;
        }
    }

    void Start()
    {
        // set player sprite/animations
        this.animator = this.GetComponent<Animator>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        switch(this.playerType)
        {
            case PlayerType.WARRIOR:
                // set Animation Controller
                this.animator.runtimeAnimatorController
                = Resources.Load<RuntimeAnimatorController>("Sprites/PlayerSprites/Animations/Warrior_Animations/AC_Warrior");
                break;
            case PlayerType.SORCERESS:
                // set Animation Controller
                this.animator.runtimeAnimatorController
                = Resources.Load<RuntimeAnimatorController>("Sprites/PlayerSprites/Animations/Sorceress_Animations/AC_Sorceress");
                break;
            default:
                Debug.Log("Error: player type is undefined.");
                break;
        }
    }

    void Update()
    {
        if (!statelock)
        {
            this.horizontal = Input.GetAxisRaw("Horizontal");
            this.vertical = Input.GetAxisRaw("Vertical");

            // attack; queue player attack
            if (Input.GetButtonDown("Fire1"))
            {
                this.CurrentState = PlayerStates.ATTACK;
                this.animator.speed = this.speed / 4;
            }
            // movement input; queue player movement
            else if ( ((this.horizontal != 0) || (this.vertical != 0)) && (!this.statelock) )
            {
                this.CurrentState = PlayerStates.WALK;
                this.animator.speed = this.speed / 2;
                this.animator.SetFloat("MoveX", this.horizontal);
                this.animator.SetFloat("MoveY", this.vertical);
            }
            // no input; queue idle
            else
            {
                this.CurrentState = PlayerStates.IDLE;
                this.animator.speed = 1;
            }
        }
    }

}