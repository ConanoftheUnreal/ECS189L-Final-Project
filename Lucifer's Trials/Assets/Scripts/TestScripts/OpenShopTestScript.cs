using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShopTestScript : MonoBehaviour
{
    [SerializeField] GameObject shop;

    // Update is called once per frame
    void Update()
    {
        // Open shop with P for now
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!shop.active)
            {
                Time.timeScale = 0;
                shop.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                shop.SetActive(false);
            }
        }
    }
}
