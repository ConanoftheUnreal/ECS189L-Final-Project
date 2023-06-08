using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType = PlayerType.WARRIOR;
    Action<Vector2> Knockback;

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
                    case PlayerStates.DEATH:
                        this.animator.Play("Death");
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

    public void PlayerDefeated()
    {
        this.animator.enabled = false;
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
        FindObjectOfType<SoundManager>().PlaySoundEffect("player hurt");
        this.playerHurt = true;
        this.CurrentState = PlayerStates.HURT;
        this.animator.speed = 1;
        // determine player death
        if (this.gameObject.GetComponent<PlayerController>().GetHealth() == 0)
        {
            // queue player death
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.statelock = false;
            this.CurrentState = PlayerStates.DEATH;
            this.animator.speed = 1;
        }
        else
        {
            // queue player hurt
            this.playerHurt = true;
            this.CurrentState = PlayerStates.HURT;
            this.animator.speed = 1;
        }

        // determine knockback handling
        switch (damageType)
        {
            case DamageTypes.CQC:
                // determine knockback by vector btw both objects
                this.animator.SetFloat("MoveX", -knockbackDirection.x);
                this.animator.SetFloat("MoveY", -knockbackDirection.y);
                if (this.playerHurt) Knockback(knockbackDirection);
                break;
            case DamageTypes.RANGED:
                // determine knockback by direction of missile
                var objVec = obj.GetComponent<Rigidbody2D>().velocity.normalized;
                this.animator.SetFloat("MoveX", -objVec.x);
                this.animator.SetFloat("MoveY", -objVec.y);
                if (this.playerHurt) Knockback(objVec);
                break;
            case DamageTypes.AOE:
                // no knockback for AOE
                // this.statelock = false (not sure if we want player locked during AOE damage)
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

        // declare function pointer for knockback call in `PlayerDamaged`
        Knockback = this.gameObject.GetComponent<PlayerMovement>().Knockback;
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
                FindObjectOfType<SoundManager>().PlaySoundEffect("knight slash");
                this.animator.speed = this.speed / 4;
                // Warrior is always 1.25 times as fast as sorceress to attack
                if (this.playerType == PlayerType.WARRIOR)
                {
                    this.animator.speed *= 1.25f;
                }
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