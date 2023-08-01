using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateLogic : MonoBehaviour
{
    // Possible item drops
    [SerializeField] GameObject coin;
    [SerializeField] GameObject gold;
    [SerializeField] GameObject gems;
    [SerializeField] GameObject healing;

    public GameObject GetEnemyDrop()
    {
        // 60% chance of a drop
        float dropRate = Random.Range(0.0f, 100.0f);
        if (dropRate < 60.0f)
        {
            // 50% coin, 30% gold, 10% gems, 10% health pot 
            float itemDropRate = Random.Range(0.0f, 100.0f);
            if (itemDropRate < 50.0f)
            {
                //Debug.Log("dropped coin");
                return coin;
            }
            else if (itemDropRate > 50.0f && itemDropRate < 80.0f)
            {
                //Debug.Log("dropped gold");
                return gold;
            }
            else if (itemDropRate > 80.0f && itemDropRate < 90.0f)
            {
                //Debug.Log("dropped gems");
                return gems;
            }
            else
            {
                //Debug.Log("dropped healing");
                return healing;
            }
        }
        else
        {
            //Debug.Log("dropped nothing");
            return null;
        }
    }
}
