using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShopTestScript : MonoBehaviour
{
    [SerializeField] GameObject shop;
    private GameObject player;

    // Update is called once per frame
    void OnTriggerEnter2D (Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            this.player = obj.gameObject;
        }
    }

    void OnTriggerExit2D()
    {
        this.player = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && this.player != null && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            shop.SetActive(true);
        }
    }
}
