using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IEnemy
{
    public float CurrentHealth { get; }

    public float TimeOnCooldown { get; }

    public bool Cooldown { get; }

    public EnemyState State { get; set; }

    public void Attack();
    public void GetMovementDirection();
}
