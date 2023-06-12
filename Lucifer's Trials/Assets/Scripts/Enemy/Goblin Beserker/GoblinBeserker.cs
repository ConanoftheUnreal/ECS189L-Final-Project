using Polarith.AI.Move;
using Polarith.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class GoblinBeserker : Enemy
{
    // The layer that the player character is on. Important for collision detection
    // algorithm used to check if player is in range.
    private static int _playerLayerMask = 1 << 7;

    // Speed of enemy may be modified by the state the enemy is in. This holds
    // that modified value.
    private float _speed;

    // The direction enemy should be move given by Polarith Context Steering Alg.
    private Vector2 _movementDirection;

    private Vector2 _centerOfRoom;

    // private bool _isAttacking;
    public void Start()
    {
        _state = EnemyState.PATROL;
        _currentHealth = stats.Health;
        _timeOnCooldown = 0;
        //_cooldown = false;
        _speed = stats.Speed;
        _player = GameObject.Find("Player");
        _centerOfRoom = Vector2.zero;
    }

    public void Awake()
    {
    }

    private void OnEnable()
    {
        if (_contextSteering == null)
            _contextSteering = GetComponentInChildren<AIMContext>();

        if (_contextSteering == null)
            Debug.LogWarning("Context Steering Not Found");

        /*if (_centerOfRoom == Vector2.zero)
        {
            setRoomCenter();
        } */
    }

    private void setRoomCenter()
    {
        GameObject rootRoom = GameObject.FindWithTag("Root");
        if (rootRoom == null)
        {
            Debug.LogWarning("rootRoom Not found");
            return;
        }
        LevelManager levelManager = rootRoom.GetComponent<LevelManager>();

        if (levelManager == null)
        {
            Debug.LogWarning("levelManager not found");
            return;
        }
        _centerOfRoom = levelManager.GetCenterOfCurrentRoom();
    }

    private void Update()
    {
        UpdateState();
        switch (_state)
        {
            case EnemyState.MOVE:
                _speed = stats.Speed;
                Move();
                break;
            case EnemyState.ATTACK:
                //Attack();
                _cooldown = true;
                break;
            case EnemyState.PATROL:
                _speed = stats.Speed * 0.8f;
                Patrol();
                break;
            case EnemyState.ORBIT:
                _speed = stats.Speed * .5f;
                Orbit();
                break;
            case EnemyState.FLEE:
                _speed = stats.Speed * 1.2f;
                Flee();
                break;
            default:
                Debug.LogWarning("Wrong State Given");
                break;
        }

        UpdateMovementDirection();
    }

    // Enable and Disables Polarith AI components for the given state.
    private void Move()
    {
        foreach (Polarith.AI.Move.AIMOrbit orbit in _contextSteering.GetComponents<Polarith.AI.Move.AIMOrbit>())
        {
            orbit.enabled = false;
        }

        foreach (Polarith.AI.Move.AIMSeek seek in _contextSteering.GetComponents<Polarith.AI.Move.AIMSeek>())
        {
            if (seek.Label == "Flee Player")
            {
                seek.enabled = false;
                continue;
            }

            seek.enabled = true;
        }

        foreach (Polarith.AI.Move.AIMWander wander in _contextSteering.GetComponents<Polarith.AI.Move.AIMWander>())
        {
            wander.enabled = false;
        }
    }

    // Enable and Disables Polarith AI components for the given state.
    private void Patrol()
    {
        foreach (Polarith.AI.Move.AIMOrbit orbit in _contextSteering.GetComponents<Polarith.AI.Move.AIMOrbit>())
        {
            orbit.enabled = false;
        }

        foreach (Polarith.AI.Move.AIMSeek seek in _contextSteering.GetComponents<Polarith.AI.Move.AIMSeek>())
        {
            if (seek.Label == "Flee Player")
            {
                seek.enabled = false;
                continue;
            }

            if (seek.Label == "Seek Player")
            {
                seek.enabled = false;
                continue;
            }

            seek.enabled = true;
        }

        foreach (Polarith.AI.Move.AIMWander wander in _contextSteering.GetComponents<Polarith.AI.Move.AIMWander>())
        {
            wander.enabled = true;
        }
    }

    // Enable and Disables Polarith AI components for the given state.
    private void Orbit()
    {
        foreach (Polarith.AI.Move.AIMOrbit orbit in _contextSteering.GetComponents<Polarith.AI.Move.AIMOrbit>())
        {
            orbit.enabled = true;
        }

        foreach (Polarith.AI.Move.AIMSeek seek in _contextSteering.GetComponents<Polarith.AI.Move.AIMSeek>())
        {
            if (seek.Label == "Flee Player" || seek.Label == "Seek Player")
            {
                seek.enabled = false;
                continue;
            }

            seek.enabled = true;
        }

        foreach (Polarith.AI.Move.AIMWander wander in _contextSteering.GetComponents<Polarith.AI.Move.AIMWander>())
        {
            wander.enabled = false;
        }
    }

    // Enable and Disables Polarith AI components for the given state.
    private void Flee()
    {
        foreach (Polarith.AI.Move.AIMOrbit orbit in _contextSteering.GetComponents<Polarith.AI.Move.AIMOrbit>())
        {
            orbit.enabled = false;
        }

        foreach (Polarith.AI.Move.AIMSeek seek in _contextSteering.GetComponents<Polarith.AI.Move.AIMSeek>())
        {
            if (seek.Label == "Flee Player")
            {
                seek.enabled = true;
                continue;
            }

            if (seek.Label == "Seek Player")
            {
                seek.enabled = false;
                continue;
            }

            seek.enabled = true;
        }

        foreach (Polarith.AI.Move.AIMWander wander in _contextSteering.GetComponents<Polarith.AI.Move.AIMWander>())
        {
            wander.enabled = false;
        }
    }

    // Updates the enemies state based on whether attack is on cooldown
    // and if player is in certain ranges.
    protected override void UpdateState()
    {
        if (_cooldown)
        {
            _timeOnCooldown += Time.deltaTime;
            if (_timeOnCooldown > Stats.Cooldown)
            {
                _cooldown = false;
                _timeOnCooldown = 0;
            }
        }

        if (_isAttacking)
        {
            return;
        }

        if (!_cooldown && GetRange(stats.Fov))
        {
            // If attack is available to use and within attack range, then attack
            // else move into attack range.
            if (GetRange(stats.AttackRange))
            {
                _state = EnemyState.ATTACK;
            }
            else
            {
                _state = EnemyState.MOVE;
            }
        }
        else if (GetRange(stats.Fov))
        {
            // If attack is unavailbe and player in field of view, then move to orbit player.
            // If in enemies attack range, quickly leave enemy attack range. If to far outside of
            // Orbit value, than move closer to orbit.
            if (GetRange(.5f))
            {
                // TODO: Need a flee range variable.
                _state = EnemyState.FLEE;
            }
            else if (!GetRange(Stats.Orbit * 1.5f))
            {
                _state = EnemyState.MOVE;
            }
            else
            {
                _state = EnemyState.ORBIT;
            }
        }
        else
        {
            _state = EnemyState.PATROL;
        }
    }

    // Returns true if player object is within range of this enemy
    protected override bool GetRange(float range)
    {
        if (_player == null)
        {
            Collider2D colliders = Physics2D.OverlapCircle(this.transform.position, range, _playerLayerMask);

            if (colliders != null)
            {
                return true;
            }

            return false;
        }

        if (Vector2.Distance(this.transform.position, _player.transform.position) <= range)
        {
            return true;
        }

        return false;
    }

    public override void Attack()
    {
        _isAttacking = true;
        //_controller.ActivateAttackSprite();
        _isAttacking = false;

        _cooldown = true;
    }

    public Vector2 MovementDirection
    {
        get { return _movementDirection; }
    }

    public float Speed
    {
        get { return _speed; }
    }

    public override void UpdateMovementDirection()
    {
        if (Mathf2.Approximately(_contextSteering.DecidedDirection.sqrMagnitude, 0))
        {
            return;
        }

        _movementDirection = _contextSteering.DecidedDirection;

        Ray theRay = new Ray(transform.position, transform.TransformDirection(_movementDirection));
        //RaycastHit2D hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(_movementDirection));

        if (Physics.Raycast(transform.position, (Vector2 )transform.position + _movementDirection, 1, layerMask: 1 << 2))
        {
            Debug.Log("here");
            if (_centerOfRoom == Vector2.zero)
            {
                setRoomCenter();
            }

            Vector2 playerToCenter = _centerOfRoom - (Vector2)transform.position;

            if (Vector3.Cross(new Vector3(_movementDirection.x, _movementDirection.y, 0), new Vector3(playerToCenter.x, playerToCenter.y, 0)).z > 0)
            {
                Debug.Log("Positive Z");
            }
            else if (Vector3.Cross(new Vector3(_movementDirection.x, _movementDirection.y, 0), new Vector3(playerToCenter.x, playerToCenter.y, 0)).z < 0)
            {
                Debug.Log("Negative Z");
            }
        }
        //Debug.Log(_contextSteering.DecidedDirection);
    }
}

/*
GameObject rootRoom = GameObject.Find("Root");
LevelManager levelManager = rootRoom.GetComponent<LevelManager>();
Vector2 roomCenter = levelManager.GetCenterOfCurrentRoom();
*/