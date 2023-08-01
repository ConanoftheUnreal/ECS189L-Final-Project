using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoblinBeserker))]
public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject poofEffect;
    GameObject player;
    Action<Vector2, int> Knockback;
    Func<bool> PlayerDefeated;

    [SerializeField] EnemyTypes enemyType;
    private bool statelock = false;
    private bool EnemyHurt = false;
    private GoblinBeserker enemy;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public enum DamageTypes { CQC, RANGED, AOE }
    public enum EnemyAnimStates { IDLE, WALK, ATTACK, HURT, DEATH }
    private EnemyAnimStates currentState;
    private EnemyAnimStates CurrentState
    {
        set
        {
            if (!this.statelock || this.EnemyHurt)
            {
                this.currentState = value;

                // Set which animation the sprite uses
                switch (this.currentState)
                {
                    case EnemyAnimStates.IDLE:
                        this.animator.Play("Idle");
                        break;
                    case EnemyAnimStates.WALK:
                        this.animator.Play("Walk");
                        break;
                    case EnemyAnimStates.ATTACK:
                        this.animator.Play("Attack");
                        this.statelock = true;
                        break;
                    case EnemyAnimStates.HURT:
                        this.animator.Play("Hurt");
                        this.statelock = true;
                        break;
                    case EnemyAnimStates.DEATH:
                        this.animator.Play("Death");
                        this.statelock = true;
                        break;
                    default:
                        Debug.Log("Error: Enemy state is undefined.");
                        break;
                }
            }
        }
    }

    void Start()
    {
        this.enemy = this.GetComponent<GoblinBeserker>();

        // Set enemy animator based on type
        this.animator = this.GetComponent<Animator>();
        switch(this.enemyType)
        {
            case EnemyTypes.BESERKER:
                // Set Animation Controller
                this.animator.runtimeAnimatorController
                = Resources.Load<RuntimeAnimatorController>("Sprites/Enemies/Goblin Beserker/Animations/AC_GoblinBerserker");
                break;
            case EnemyTypes.SLINGER:
                // Set Animation Controller
                this.animator.runtimeAnimatorController
                = Resources.Load<RuntimeAnimatorController>("Sprites/Enemies/Goblin Slinger/Animations/AC_GoblinSlinger");
                break;
            default:
                Debug.Log("Error: Enemy type is undefined.");
                break;
        }
        // Start facing down
        this.animator.SetFloat("MoveX", 0.0f);
        this.animator.SetFloat("MoveY", -2.0f);

        // Reference to player
        player = GameObject.Find("Player");
        // Function pointer for knockback
        Knockback = this.gameObject.GetComponent<EnemyMovement>().Knockback;
        // Function pointer to determine if player has been defeated
        PlayerDefeated = player.GetComponent<PlayerController>().PlayerDefeated;
    }

    public void AttackFinished()
    {
        this.statelock = false;
    }

    public void HurtFinished()
    {
        this.statelock = false;
        this.EnemyHurt = false;
    }

    public bool GetStateLock()
    {
        return this.statelock;
    }

    public EnemyTypes GetEnemyType()
    {
        return this.enemyType;
    }

    public void StepSound()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Enemy Walk");
    }

    public void EnemyDamaged(GameObject obj, bool killed, bool wasProjectile, int damage)
    {
        // Ensure attack sprite is never active during attack interruption
        this.gameObject.GetComponent<EnemyAttackController>().DeactivateAttackSprite();

        // determine enemy death
        if (killed)
        {
            // queue player death
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.statelock = false;
            this.CurrentState = EnemyAnimStates.DEATH;
            this.animator.speed = 1;
            // play death effect
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
            FindObjectOfType<SoundManager>().PlaySoundEffect("Enemy Defeated");
            return;
        }
        else
        {
            // queue player hurt
            this.EnemyHurt = true;
            this.CurrentState = EnemyAnimStates.HURT;
            this.animator.speed = 1;
        }

        // determine knockback handling
        if (wasProjectile)
        {
            // determine knockback by direction of missile
            var objVec = obj.GetComponent<Rigidbody2D>().velocity.normalized;
            this.animator.SetFloat("MoveX", -objVec.x);
            this.animator.SetFloat("MoveY", -objVec.y);
            // projectiles provide less push
            Knockback(objVec, damage/2);
        }
        else
        {
            // determine knockback by vector btw player and enemy
            // point closest to player of enemy
            var collisionPt = obj.transform.parent.gameObject.GetComponent<Collider2D>().ClosestPoint(this.gameObject.transform.position);
            // vector towards enemy from player
            var knockbackDirection = ((Vector2)this.gameObject.transform.position - collisionPt).normalized;
            this.animator.SetFloat("MoveX", -knockbackDirection.x);
            this.animator.SetFloat("MoveY", -knockbackDirection.y);
            Knockback(knockbackDirection, damage);
        }

    }

    void Update()
    {
        if (PlayerDefeated())
        {
            this.CurrentState = EnemyAnimStates.IDLE;
            this.animator.speed = 1;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            statelock = true;
        }
        else if (!statelock)
        {
            var horizontal = this.enemy.MovementDirection.x;
            var vertical = this.enemy.MovementDirection.y;

            var targetDistance = (player.transform.position - this.transform.position);

            // Attacking
            if (this.enemy.State == EnemyState.ATTACK)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                this.CurrentState = EnemyAnimStates.ATTACK;
                this.animator.speed = 1;
                this.animator.SetFloat("MoveX", targetDistance.normalized.x * 2);
                this.animator.SetFloat("MoveY", targetDistance.normalized.y * 2);
            }
            // Patrolling
            else if ( ((horizontal != 0) || (vertical != 0)) && (!this.statelock) )
            {
                this.CurrentState = EnemyAnimStates.WALK;
                this.animator.speed = this.enemy.Speed / 2;
                this.animator.SetFloat("MoveX", horizontal * 2);
                this.animator.SetFloat("MoveY", vertical * 2);
            }
            // Moving, Orbiting, Fleeing
            else if (this.enemy.State == EnemyState.ORBIT || this.enemy.State == EnemyState.FLEE)
            {
                this.CurrentState = EnemyAnimStates.WALK;
                this.animator.speed = this.enemy.Speed / 2;
                this.animator.SetFloat("MoveX", targetDistance.normalized.x * 2);
                this.animator.SetFloat("MoveY", targetDistance.normalized.y * 2);
            }
            // Idling
            else
            {
                this.CurrentState = EnemyAnimStates.IDLE;
                this.animator.speed = 1;
            }
        }
    }
}