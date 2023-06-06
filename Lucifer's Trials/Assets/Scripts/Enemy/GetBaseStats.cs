using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GetBaseStats
{
    [SerializeField] private bool useConstant;
    [SerializeField] private float maxHealth;

    // Movement Speed.
    [SerializeField] private float speed;

    // Attack Cooldown.
    [SerializeField] private float cooldown;

    // Range in which enemy sees player character.
    [SerializeField] private float fov;

    // Distance enemy wants to be from plyaer character when attack on cooldown.
    [SerializeField] private float orbit;

    // Distance from player character that enemy will decide to attack.
    [SerializeField] private float attackRange;

    [SerializeField] private float damage;

    [SerializeField] private BaseStats baseStats;

    public float Damage
    {
        get { return useConstant ? damage : baseStats.Damage; }
    }

    // Use these functions to get base stats of creature. Can also be used later 
    // to calculate increase in stats based on player power level.
    public float Health
    {
        get { return useConstant ? maxHealth : baseStats.MaxHealth; }
    }

    public float Speed
    {
        get { return useConstant ? speed : baseStats.Speed; }
    }

    public float Cooldown
    {
        get { return useConstant ? cooldown : baseStats.Cooldown; }
    }

    public float Fov
    {
        get { return useConstant ? fov : baseStats.FoV; }
    }

    public float Orbit
    {
        get { return useConstant ? orbit : baseStats.Orbit; }
    }

    public float AttackRange
    {
        get { return useConstant ? attackRange : baseStats.AttackRange; }
    }
}
