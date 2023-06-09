using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BaseStats will hold the values of stats on an enemy that will never change throughout the game.
// CreateAssetMenu allows it be a prefab.
[CreateAssetMenu]
public class BaseStats : ScriptableObject
{
    public float MaxHealth;

    // Movement Speed.
    public float Speed;

    // Attack Cooldown.
    public float Cooldown;

    // Range in which enemy sees player character.
    public float FoV;

    // Distance enemy wants to be from plyaer character when attack on cooldown.
    public float Orbit;

    // Distance from player character that enemy will decide to attack.
    public float AttackRange;

    public float Damage;
}
