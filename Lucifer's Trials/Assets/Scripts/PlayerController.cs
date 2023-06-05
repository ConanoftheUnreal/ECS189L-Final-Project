using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class PlayerController : MonoBehaviour
{
    private int health;
    private int SP;

    // Start is called before the first frame update
    void Start()
    {
        this.health = 3;
    }

    public void IncreaseHealth(int amount)
    {
        this.health += amount;
        //Debug.Log(this.health);
    }

    public void DecreaseHealth(int amount)
    {
        this.health -= amount;
        if (this.health < 0)
        {
            this.health = 0;
        }
        //Debug.Log(this.health);
    }

    public int GetHealth()
    {
        return this.health;
    }
     public void IncreaseSP(int amount)
    {
        this.SP += amount;
        //Debug.Log(this.SP);
    }
    public int GetSP()
    {
        return this.SP;
    }
}
