using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType = PlayerType.WARRIOR;

    private bool statelock = false;
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
            if (!this.statelock)
            {
                this.currentState = value;

                // set which animation the sprite uses
                switch(this.currentState)
                {
                    case PlayerStates.IDLE:
                        this.animator.Play("Idle");
                        break;
                    case PlayerStates.WALK:
                        this.animator.Play("Walk");
                        break;
                    case PlayerStates.ATTACK:
                        this.animator.Play("Attack1");
                        statelock = true;
                        break;
                    default:
                        Debug.Log("Error: player state is undefined.");
                        break;
                }
            }
        }
    }

    void AttackFinished()
    {
        this.statelock = false;
    }

    public bool GetStateLock()
    {
        return this.statelock;
    }

    public PlayerType GetPlayerType()
    {
        return this.playerType;
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
        this.horizontal = Input.GetAxisRaw("Horizontal");
        this.vertical = Input.GetAxisRaw("Vertical");

         if (Input.GetButtonDown("Fire1"))
        {
            this.CurrentState = PlayerStates.ATTACK;
            this.animator.speed = this.speed;
        }
        // player movement
        else if ( ((this.horizontal != 0) || (this.vertical != 0)) && (!this.statelock) )
        {
            this.CurrentState = PlayerStates.WALK;
            this.animator.speed = this.speed / 2;
            this.animator.SetFloat("MoveX", this.horizontal);
            this.animator.SetFloat("MoveY", this.vertical);
        }
        else
        {
            this.CurrentState = PlayerStates.IDLE;
            this.animator.speed = 1;
        }
    }

}