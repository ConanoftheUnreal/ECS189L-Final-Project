using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPDisplay : MonoBehaviour
{
    private int SP;
    public Text spText;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.spText.text = this.player.GetComponent<PlayerController>().GetHealth().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        this.spText.text = "SP : " +  this.player.GetComponent<PlayerController>().GetSP().ToString();
        //Temporary testing function for gaining SP
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SP--;
        }


    }
}