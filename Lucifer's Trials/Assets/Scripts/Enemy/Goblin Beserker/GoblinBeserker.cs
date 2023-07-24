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

    private Vector2 _lastPlayerPosition;
    private GameObject _lastPlayerRepresentation;
    private bool _wallBetweenPlayerEnemy = false;
    private bool sighted = false;
    private float timeInSearch;

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
        _lastPlayerRepresentation = new GameObject("Last Player Location");
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
    }

    public float MaxHealth
    {
        set {
            float tmp = stats.Health;
            stats.Health = value;
            _currentHealth += stats.Health - tmp;
        }

        get { return stats.Health; }
    }

    public float Damage
    {
        set
        {
            stats.Damage = value;
        }

        get { return stats.Damage; }
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
        // If this returns true, then the player is in range but there is a wall between
       _wallBetweenPlayerEnemy = WallBetweenPlayerandEnemy(Stats.Fov);
        UpdateState();

        if (_state != EnemyState.SEARCH)
        {
            timeInSearch = 0;
        }

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
            case EnemyState.SEARCH:
                _speed = stats.Speed;
                //TestProjection(_lastPlayerRepresentation.transform.position);
                Search();
                break;
            default:
                Debug.LogWarning("Wrong State Given");
                break; 
        }
        
        UpdateMovementDirection();
    }

    private void TestProjection(Vector3 o)
    {
        Vector3 a = o - transform.position;
        Vector3 p = _player.transform.position - transform.position;
        Vector3 projection = Vector3.Project(p, a);
        Vector3 error = p - projection;

        Debug.DrawRay(transform.position, p, Color.magenta);
        Debug.DrawRay(transform.position, projection, Color.white);
        Debug.DrawRay(this.transform.position, a, Color.magenta);
        Debug.DrawRay(projection + this.transform.position, error, Color.white);
        Debug.Log(error);

        //_lastPlayerRepresentation.transform.Translate(-error);
        _lastPlayerRepresentation.transform.position += error;
        

        /*RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, a + newPos, (a + newPos).magnitude);

        bool wallHit = false;
        foreach (RaycastHit2D ray in hit)
        {

            if (ray.collider.gameObject.name == "Collision")
            {
                    // Then the gameObject is in a wall...try again?
                    Debug.Log("Collided 2 times ya here");
                    TestProjection(new Vector2(o.x * .75f, o.y * .75f));
            }
        }*/
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

            if (seek.Label == "Seek Player")
            {
                seek.enabled = true;
                seek.GameObjects[0] = _player;
                continue;
            }

            seek.enabled = true;
        }

        foreach (Polarith.AI.Move.AIMWander wander in _contextSteering.GetComponents<Polarith.AI.Move.AIMWander>())
        {
            wander.enabled = false;
        }
    }

    private void OnDisable()
    {
        if (_lastPlayerRepresentation != null)
        {
            _lastPlayerRepresentation.SetActive(false);
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

    private void Search()
    {
        _lastPlayerRepresentation.transform.position = _lastPlayerPosition;

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
                seek.enabled = true;
                seek.GameObjects[0] = _lastPlayerRepresentation;
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
        if (_cooldown)
        {
            _timeOnCooldown += Time.deltaTime;
            if (_timeOnCooldown > Stats.Cooldown)
            {
                _cooldown = false;
                _timeOnCooldown = 0;
            }
        }

        if (GetRange(stats.Fov))
        {
            if (_wallBetweenPlayerEnemy)
            {
                if (sighted)
                {
                    if (Vector2.Distance(this.transform.position, _lastPlayerRepresentation.transform.position) <= .25)
                    {
                        sighted = false;
                        _state = EnemyState.PATROL;
                    }
                    else
                    {
                        _state = EnemyState.SEARCH;
                        timeInSearch += Time.deltaTime;
                    }
                }
                else
                {
                    sighted = false;
                    _state = EnemyState.PATROL;
                }
            }
            else
            {
                sighted = true;
                if (_cooldown)
                {
                    if (GetRange(Stats.Orbit * 1.5f))
                    {
                        _state = EnemyState.ORBIT;
                    }
                    else
                    {
                        _state = EnemyState.MOVE;
                    }
                }
                else
                {
                    if (GetRange(Stats.AttackRange))
                    {
                        _state = EnemyState.ATTACK;
                    }
                    else
                    {
                        _state = EnemyState.MOVE;
                    }
                }
            }
        }
        else
        {
            sighted = false;
            _state = EnemyState.PATROL;
        }

        if (timeInSearch > 2)
        {
            _state = EnemyState.PATROL;
            sighted = false;
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

        //Debug.Log(_contextSteering.DecidedDirection);
    }

    private bool WallBetweenPlayerandObject(GameObject gameObject)
    {
        Vector3 direction = (gameObject.transform.position - transform.position).normalized;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction, Vector2.Distance(this.transform.position, gameObject.transform.position));
        // Debug.DrawRay(transform.position, direction);

        foreach (RaycastHit2D ray in hit)
        {
            if (ray.collider.gameObject.name == "Collision")
            {
                // Debug.Log("Hella True");
                return true;
            }
        }

        return false;
    }

    private bool WallBetweenPlayerandEnemy(float distance)
    {
        Vector3 playerDirection = (_player.transform.position - transform.position).normalized;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, playerDirection, distance);
        // Debug.DrawRay(transform.position, playerDirection);

        bool playerHit = false;
        bool wallHit = false;
        foreach (RaycastHit2D ray in hit)
        {

            if (ray.collider.gameObject.name == "PlayerHurtbox")
            {
                // Should only work if ray collides with player before a wall
                if (!playerHit && !wallHit)
                {
                    _lastPlayerPosition = _player.transform.position;
                }

                playerHit = true;
            }

            if (ray.collider.gameObject.name == "Collision")
            {
                wallHit = true;
                if (!playerHit)
                {
                    if (_state != EnemyState.SEARCH)
                    {
                        _lastPlayerRepresentation.transform.position = _lastPlayerPosition;
                    }

                    return true;
                }
            }
        }

        return false;
    }

    private void OnDestroy()
    {
        if (_lastPlayerRepresentation != null)
        {
            Destroy(_lastPlayerRepresentation.gameObject);
        }
        
    }
}

/*
GameObject rootRoom = GameObject.Find("Root");
LevelManager levelManager = rootRoom.GetComponent<LevelManager>();
Vector2 roomCenter = levelManager.GetCenterOfCurrentRoom();
*/