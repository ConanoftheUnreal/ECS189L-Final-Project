using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private int health;
    public Text healthText;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.healthText.text = this.player.GetComponent<PlayerController>().GetHealth().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        this.healthText.text = "HEALTH : " +  this.player.GetComponent<PlayerController>().GetHealth().ToString();
        //Temporary testing function for taking damage
        if (Input.GetKeyDown(KeyCode.Q))
        {
            health--;
        }


    }
}
