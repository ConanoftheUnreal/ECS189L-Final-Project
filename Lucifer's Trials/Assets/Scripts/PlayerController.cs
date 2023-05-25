using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        this.health = 3;
    }

    public void IncreaseHealth(int amount)
    {
        this.health += amount;
        Debug.Log(this.health);
    }

    public int GetHealth()
    {
        return this.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
