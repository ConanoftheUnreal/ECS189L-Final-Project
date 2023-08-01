using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Lucifer;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType = PlayerType.WARRIOR;
    [SerializeField] GameObject dashEffect;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject noAttackIndicatorPrefab;
    GameObject noAttackIndicator;
    Action<Vector2> Knockback;
    Action DeactivateAttackSprite;
    Func<int, int> DecreaseHealth;
    Func<bool> IsDashing;
    Func<bool> GetMoveLocked;

    private bool statelock = false;
    private bool playerHurt = false;
    private bool hurtable = true;
    private float deathTime = 0.5f;
    private float currentTime = 0.0f;
    private float sinceHurt = 0.0f;
    private float horizontal;
    private float vertical;
    private Rigidbody2D rb;
    private Vector3 noAttackIndicatorPos = new Vector3(0f, 0.8f, 0f);
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

                // Set which animation the sprite uses
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
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
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

    public void StepSound()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Player Walk");
    }

    public void PlayDashEffect()
    {
        float angle = Vector2.SignedAngle(Vector2.right, this.rb.velocity);
        GameObject effect = (GameObject)Instantiate(this.dashEffect, this.transform.position, Quaternion.Euler(0, 0, angle));
    }

    private void InvincibilityFrames()
    {
        var spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (this.sinceHurt >= 0.5f)
        {
            this.hurtable = true;
            Destroy(this.noAttackIndicator);

            var color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
        }
        else
        {
            // Attack indicator handling
            if (this.noAttackIndicator == null)
            {
                noAttackIndicator = (GameObject)Instantiate(this.noAttackIndicatorPrefab, this.transform.position + this.noAttackIndicatorPos, Quaternion.identity);
            }
            this.noAttackIndicator.transform.position = this.transform.position + this.noAttackIndicatorPos;

            // Alpha value blinking
            spriteRenderer.color += new Color (0, 0, 0, 1f) * 2 * Time.deltaTime;
            if (spriteRenderer.color.a >= 1f)
            {
                spriteRenderer.color = new Color (1f, 1f, 1f, 0.5f);
            }
            this.sinceHurt += Time.deltaTime;
        }
    }

    public bool PlayerDamaged(GameObject obj, int damage, DamageTypes damageType)
    {
        // Ensure attack sprite is never active during attack interruption
        DeactivateAttackSprite();

        // Invincibility frames
        if (IsDashing() || !this.hurtable) return false;

        // Determine location of collision relative to player
        var collisionPt = obj.GetComponent<Collider2D>().ClosestPoint(this.gameObject.transform.position);
        var knockbackDirection = ((Vector2)this.gameObject.transform.position - collisionPt).normalized;
        
        // Determine player death
        var health = DecreaseHealth(damage);
        if (health == 0)
        {
            // Queue player death
            FindObjectOfType<SoundManager>().StopCurrentTrack();
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.statelock = false;
            this.CurrentState = PlayerStates.DEATH;
            this.animator.speed = 1;
            // Play death effect
            GameObject effect;
            if (UnityEngine.Random.Range(0.0f, 100.0f) < 50.0f)
            {
                effect = (GameObject)Instantiate(this.deathEffect, this.transform.position - (new Vector3(0.5f, 0, 0)), Quaternion.identity);
            }
            else
            {
                effect = (GameObject)Instantiate(this.deathEffect, this.transform.position + (new Vector3(0.5f, 0, 0)), Quaternion.identity);
                effect.GetComponent<SpriteRenderer>().flipX = true;
            }
            return true;
        }
        else
        {
            // Queue player hurt
            this.playerHurt = true;
            this.hurtable = false;
            this.sinceHurt = 0.0f;
            this.CurrentState = PlayerStates.HURT;
            this.animator.speed = 1;
        }

        // Determine knockback handling
        switch (damageType)
        {
            case DamageTypes.CQC:
                // Determine knockback by vector btw both objects
                this.animator.SetFloat("MoveX", -knockbackDirection.x);
                this.animator.SetFloat("MoveY", -knockbackDirection.y);
                Knockback(knockbackDirection);
                break;
            case DamageTypes.RANGED:
                // Determine knockback by direction of missile
                var objVec = obj.GetComponent<Rigidbody2D>().velocity.normalized;
                this.animator.SetFloat("MoveX", -objVec.x);
                this.animator.SetFloat("MoveY", -objVec.y);
                Knockback(objVec);
                break;
            case DamageTypes.COLLIDE:
                var hitDirection = (this.transform.position - obj.transform.position).normalized;
                Knockback(hitDirection);
                break;
            default:
                Debug.Log("Error: damage type is undefined.");
                break;
        }

        return true;
    }

    public void SetClass(PlayerType playerType)
    {
        this.playerType = playerType;
    }

    void Start()
    {
        // Determine how to animate player
        StartNewAnimation();

        this.rb = this.gameObject.GetComponent<Rigidbody2D>();

        // Declare function pointer for knockback call in `PlayerDamaged`
        Knockback = this.gameObject.GetComponent<PlayerMovement>().Knockback;
        // Declare function pointer to determine player dashing
        IsDashing = this.gameObject.GetComponent<PlayerMovement>().IsDashing;
        // Declare function pointer to hurt player
        DecreaseHealth = this.gameObject.GetComponent<PlayerController>().DecreaseHealth;
        // Declare function pointer to deactivate attack sprite
        DeactivateAttackSprite = this.gameObject.GetComponent<PlayerAttackController>().DeactivateAttackSprite;
        // Declare function pointer to determine if movement of player is locked
        GetMoveLocked = this.gameObject.GetComponent<PlayerMovement>().GetMoveLocked;
    }

    // Function redefines all the private fields that are required to be changed based on the PlayerType
    public void StartNewAnimation()
    {
        // Set player sprite/animations
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
        // Start facing down
        this.animator.SetFloat("MoveX", 0.0f);
        this.animator.SetFloat("MoveY", -2.0f);
    }

    void Update()
    {
        if (this.animator.enabled == false)
        {
            if (this.currentTime >= this.deathTime)
            {
                this.gameObject.GetComponent<PlayerController>().SetDead(true);
                SceneManager.LoadScene("DeathScreen");
            }
            this.currentTime += Time.deltaTime;
        }

        if (!this.hurtable && !this.playerHurt)
        {
            InvincibilityFrames();
        }

        if (!statelock && !GetMoveLocked() && Time.timeScale == 1)
        {
            this.horizontal = Input.GetAxisRaw("Horizontal");
            this.vertical = Input.GetAxisRaw("Vertical");

            // Attack; queue player attack
            if (Input.GetButtonDown("Fire1") && this.hurtable && !this.IsDashing())
            {
                this.CurrentState = PlayerStates.ATTACK;
                this.animator.speed = this.GetComponent<PlayerController>().GetSpeed() / 4;
                // Player type specific changes
                switch (playerType)
                {
                    case PlayerType.WARRIOR:
                        this.animator.speed *= 1.8f;
                        FindObjectOfType<SoundManager>().PlaySoundEffect("Slash");
                        break;
                    case PlayerType.SORCERESS:
                        FindObjectOfType<SoundManager>().PlaySoundEffect("Fireball");
                        break;
                    default:
                        Debug.Log("Error: Invalid player type.");
                        break;
                }
            }
            // Movement input; queue player movement
            else if ( ((this.horizontal != 0) || (this.vertical != 0)) && (!this.statelock) )
            {
                this.CurrentState = PlayerStates.WALK;
                this.animator.speed = ((this.GetComponent<PlayerController>().GetSpeed() - 5) / 4) + 2.5f;
                this.animator.SetFloat("MoveX", this.horizontal * 2);
                this.animator.SetFloat("MoveY", this.vertical * 2);
            }
            // No input; queue idle
            else
            {
                this.CurrentState = PlayerStates.IDLE;
                this.animator.speed = 1;
            }
        }
    }

}