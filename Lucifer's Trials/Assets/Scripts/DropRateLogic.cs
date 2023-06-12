using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateLogic : MonoBehaviour
{
    // Possible item drops
    [SerializeField] GameObject coin;
    [SerializeField] GameObject gold;
    [SerializeField] GameObject gems;
    [SerializeField] GameObject heart;
    [SerializeField] GameObject sword;

    public GameObject GetEnemyDrop()
    {
        // 50% chance of a drop
        float dropRate = Random.Range(0.0f, 100.0f);
        if (dropRate > 50.0f) 
        {
            // 10% to drop either max health or attack upgrade
            float upgradeDropRate = Random.Range(0.0f, 100.0f);
            if (upgradeDropRate <= 10.0f)
            {
                return sword;
            }
            else if (upgradeDropRate >= 90.0f)
            {
                return heart;
            }

            // 50% coin, 30% gold, 20% gems 
            float currencyDropRate = Random.Range(0.0f, 100.0f);
            if (currencyDropRate <= 50.0f)
            {
                return coin;
            }
            else if (currencyDropRate >= 70.0f)
            {
                return gold;
            }
            else
            {
                return gems;
            }
        }
        else
        {
            return null;
        }
    }
}
