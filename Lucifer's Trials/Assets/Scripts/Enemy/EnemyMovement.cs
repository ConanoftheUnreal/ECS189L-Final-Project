using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoblinBeserker))]
[RequireComponent(typeof(EnemyAnimation))]
public class EnemyMovement : MonoBehaviour
{
    Func<bool> GetStateLock;
    private bool statelock = false;
    private float knockbackForce = 2f;
    private Rigidbody2D rb;
    private GoblinBeserker enemy;

    public void Start()
    {
        this.rb = this.gameObject.GetComponent<Rigidbody2D>();
        this.enemy = this.gameObject.GetComponent<GoblinBeserker>();

        this.GetStateLock = this.gameObject.GetComponent<EnemyAnimation>().GetStateLock;
    }

    public void Knockback(Vector2 direction, int damage)
    {
        this.rb.velocity = direction * (new Vector2(knockbackForce * damage, knockbackForce * damage));
    }

    public void Update()
    {
        statelock = GetStateLock();
        if (!statelock)
        {
            this.rb.velocity = this.enemy.MovementDirection.normalized * this.enemy.Speed;
        }
    }
}