using Polarith.AI.Move;
using Polarith.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GoblinBeserker : Enemy
{
    private static int _playerLayerMask = 1 << 7;
    private float _speed;
    private Vector2 _movementDirection;


    public void Start()
    {
        _currentHealth = stats.Health;
        _timeOnCooldown = 0;
        //_cooldown = false;
        _speed = stats.Speed;
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
        Debug.Log(_state.ToString());
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

        GetMovementDirection();
    }

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

    protected override void UpdateState()
    {
        if (!_cooldown && GetRange(stats.Fov))
        {
            _state = EnemyState.ATTACK;

            if (GetRange(stats.AttackRange))
            {
               /* Do an attack action */
            }
            else
            {
                _state = EnemyState.MOVE;
            }
        }
        else if (GetRange(stats.Fov))
        {
            if (GetRange(.5f))
            {
                _state = EnemyState.FLEE;
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

    public override void GetMovementDirection()
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
