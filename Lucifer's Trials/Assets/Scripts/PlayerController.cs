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
        FindObjectOfType<SoundManager>().PlayMusicTrack("game theme");
        this.health = 3;
    }

    public void IncreaseHealth(int amount)
    {
        this.health += amount;
        Debug.Log(this.health);
    }

    public void DecreaseHealth(int amount)
    {
        this.health -= amount;
        Debug.Log(this.health);
    }

    public int GetHealth()
    {
        return this.health;
    }
     public void IncreaseSP(int amount)
    {
        this.SP += amount;
        Debug.Log(this.SP);
    }
    public int GetSP()
    {
        return this.SP;
    }
}
