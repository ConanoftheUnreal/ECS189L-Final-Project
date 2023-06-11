using Polarith.AI.Move;
using Polarith.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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


    public void Start()
    {
        _state = EnemyState.PATROL;
        _currentHealth = stats.Health;
        _timeOnCooldown = 0;
        //_cooldown = false;
        _speed = stats.Speed;
        _player = GameObject.Find("Player");
    }

    private void OnEnable()
    {
        if (_contextSteering == null)
            _contextSteering = GetComponentInChildren<AIMContext>();

        if (_contextSteering == null)
            Debug.LogWarning("Context Steering Not Found");
    }

    private void Update()
    {
        UpdateState();
        switch (_state)
        {
            case EnemyState.MOVE:
                Move();
                break;    
            case EnemyState.ATTACK:
                break;
            case EnemyState.PATROL:
                _speed = _speed * 0.8f;
                Patrol();
                break;
            case EnemyState.ORBIT:
                _speed = _speed * .5f;
                Orbit();
                break;
            case EnemyState.FLEE:
                _speed = _speed * 1.2f;
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
            if (GetRange(.3f))
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
        /* Do the Attack Thing */
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
            Debug.Log("here");
            return;
        }

        //Debug.Log(_contextSteering.DecidedDirection);
        _movementDirection = _contextSteering.DecidedDirection;
    }
}
