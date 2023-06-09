using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class PlayerController : MonoBehaviour
{
    private int maxHealth;
    private int maxSP;
    private int health;
    private int SP;
    private int wallet;

    // Start is called before the first frame update
    void Start()
    {
        this.maxHealth = 10;
        this.maxSP = 5;

        this.health = 10;

        // begin background music
        FindObjectOfType<SoundManager>().PlayMusicTrack("game theme");
    }

    public bool AtMaxHealth()
    {
        return (health == maxHealth);
    }

    public bool AtMaxSP()
    {
        return (SP == maxSP);
    }

    public void IncreaseHealth(int amount)
    {
        this.health += amount;
        if (this.health > maxHealth)
        {
            this.health = maxHealth;
        }
    }

    public int DecreaseHealth(int amount)
    {
        this.health -= amount;
        if (this.health < 0)
        {
            this.health = 0;
        }
        return this.health;
    }

    public int GetHealth()
    {
        return this.health;
    }

    public void IncreaseSP(int amount)
    {
        this.SP += amount;
        if (this.SP > maxSP)
        {
            this.SP = maxSP;
        }
    }

    public void DecreaseSP(int amount)
    {
        this.SP -= amount;
        if (this.SP < 0)
        {
            this.SP = 0;
        }
    }

    public int GetSP()
    {
        return this.SP;
    }

    public void IncreaseWallet(int amount)
    {
        this.wallet += amount;
        Debug.Log("Current Gold: " + this.wallet);
    }

    // Realizing this is not way to do this; we do not ensure that when the Player is destroyed
    // the Bank is not. This causes a big red warning atm, since when we exit simulation, it destroys
    // all gameobjects simultaneously.

    // void OnDestroy()
    // {
    //     GameObject.Find("Bank").GetComponent<BankData>().Deposit(this.wallet);
    // }

    public int GetWallet()
    {
        return this.wallet;
    }
}
