using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankData : MonoBehaviour
{
    public static BankData instance;
    [SerializeField] private int money = 0;

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

    public void Deposite(int moneyIn)
    {
        BankData.instance.money += moneyIn;
    }

    public int getMoney()
    {
        return this.money;
    }

    void Start()
    {
        this.money = BankData.instance.money;
    }
}
