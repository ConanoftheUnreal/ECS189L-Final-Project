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
    private bool dead = false;

    [SerializeField] private PlayerStats playerStats;

    void Awake()
    {
        this.gameObject.GetComponent<PlayerAnimationController>().SetClass(this.playerStats.playerType);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Determine the class of the player along with their stats
        newClass();

        // begin background music
        FindObjectOfType<SoundManager>().PlayMusicTrack("Game Theme");
    }

    public void newClass()
    {
        var playerType = this.gameObject.GetComponent<PlayerAnimationController>().GetPlayerType();
        switch(playerType)
        {
            case PlayerType.WARRIOR:
                this.maxHealth = 12 + this.playerStats.maxHealthIncrease;
                this.health = this.maxHealth;
                //this.maxSP = 5;
                this.wallet = 0 + this.playerStats.wallet;
                this.attack = 2 + this.playerStats.attackIncrease;
                this.speed = 5 + this.playerStats.speedIncrease;
                break;
            case PlayerType.SORCERESS:
                this.maxHealth = 7 + this.playerStats.maxHealthIncrease;
                this.health = this.maxHealth;
                //this.maxSP = 5;
                this.wallet = 0 + this.playerStats.wallet;
                this.attack = 1 + this.playerStats.attackIncrease;
                this.speed = 7 + this.playerStats.speedIncrease;

                break;
            default:
                Debug.Log("Error: player type is undefined.");
                break;
        }
    }

    public bool AtMaxHealth()
    {
        return (health == maxHealth);
    }

    public bool PlayerDefeated()
    {
        return (health == 0);
    }

    // Needs to be removed.
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

    // Needs to be removed.
    public void IncreaseSP(int amount)
    {
        this.SP += amount;
        if (this.SP > maxSP)
        {
            this.SP = maxSP;
        }
    }

    // Needs to be removed.
    public void DecreaseSP(int amount)
    {
        this.SP -= amount;
        if (this.SP < 0)
        {
            this.SP = 0;
        }
    }

    // Needs to be removed.
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

    public void SetDead(bool state)
    {
        this.dead = state;
    }

    void OnDestroy()
    {
        switch(this.playerStats.playerType)
        {
            case PlayerType.WARRIOR:
                this.playerStats.maxHealthIncrease = this.maxHealth - 12;

                if (this.dead)
                {
                    this.playerStats.wallet = this.wallet - (this.wallet - this.playerStats.wallet)/2;
                }
                else
                {
                    this.playerStats.wallet = this.wallet;
                }

                this.playerStats.attackIncrease = this.attack - 2;
                this.playerStats.speedIncrease = this.speed - 5;
                break;
            case PlayerType.SORCERESS:
                this.playerStats.maxHealthIncrease = this.maxHealth - 7;

                if (this.dead)
                {
                    this.playerStats.wallet = this.wallet - (this.wallet - this.playerStats.wallet)/2;
                }
                else
                {
                    this.playerStats.wallet = this.wallet;
                }

                this.playerStats.attackIncrease = this.attack - 1;
                this.playerStats.speedIncrease = this.speed - 7;

                break;
            default:
                Debug.Log("Error: player type is undefined.");
                break;
        }

        // edge case for certain crashes
        if (this.playerStats.maxHealthIncrease < 0) this.playerStats.maxHealthIncrease = 0;
        if (this.playerStats.wallet < 0) this.playerStats.wallet = 0;
        if (this.playerStats.attackIncrease < 0) this.playerStats.attackIncrease = 0;
        if (this.playerStats.speedIncrease < 0) this.playerStats.speedIncrease = 0;
    }

}
