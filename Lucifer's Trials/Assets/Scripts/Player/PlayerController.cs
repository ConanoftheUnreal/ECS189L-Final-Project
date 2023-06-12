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
    private int attack;
    private int speed;
    private int wallet;

    private PlayerController instance;

    void Awake() {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var playerType = this.gameObject.GetComponent<PlayerAnimationController>().GetPlayerType();

        switch(playerType)
        {
            case PlayerType.WARRIOR:
                this.maxHealth = 12;    // originally 10
                this.health = 12;
                //this.maxSP = 5;
                this.wallet = 0;
                this.attack = 2; // originally 3
                this.speed = 5; // originally 6
                break;
            case PlayerType.SORCERESS:
                this.maxHealth = 7;
                this.health = 7;
                //this.maxSP = 5;
                this.wallet = 0;
                this.attack = 1;
                this.speed = 8;

                break;
            default:
                Debug.Log("Error: player type is undefined.");
                break;
        }
        // begin background music
        FindObjectOfType<SoundManager>().PlayMusicTrack("Game Theme");
    }

    public bool AtMaxHealth()
    {
        return (health == maxHealth);
    }

    public bool PlayerDefeated()
    {
        return (health == 0);
    }

    public bool AtMaxSP()
    {
        return (SP == maxSP);
    }

    public void IncreaseMaxHealth(int amount)
    {
        this.health += amount;
        this.maxHealth += amount;
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

    public int GetMaxHealth()
    {
        return this.maxHealth;
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

    public void DecreaseWallet(int amount)
    {
        this.wallet -= amount;
    }

    public int GetWallet()
    {
        return this.wallet;
    }

    public void IncreaseAttack(int amount)
    {
        this.attack += amount;
    }

    public int GetAttack()
    {
        return this.attack;
    }

    public void IncreaseSpeed(int amount)
    {
        this.speed += amount;
    }

    public int GetSpeed()
    {
        return this.speed;
    }
}
