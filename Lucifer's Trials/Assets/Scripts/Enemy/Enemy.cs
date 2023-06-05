using Polarith.AI.Move;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected GetBaseStats stats;
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected float _timeOnCooldown;
    [SerializeField] protected bool _cooldown;
    [SerializeField] protected bool _isAttacking;
    [SerializeField] protected EnemyState _state = EnemyState.PATROL;
    [SerializeField] protected GameObject _player;
    [SerializeField] protected AIMContext _contextSteering;

    public float CurrentHealth
    {
        get { return _currentHealth; }
    }

    public float TimeOnCooldown
    {
        get { return _timeOnCooldown; }
    }

    public bool Cooldown
    {
        get { return _cooldown; }
    }

    public EnemyState State
    {
        get => _state; set => _state = value;
    }

    public GetBaseStats Stats
    {
        get => stats;
    }

    public abstract void Attack();
    public abstract void GetMovementDirection();
    protected abstract bool GetRange(float range);
    protected abstract void UpdateState();
}
