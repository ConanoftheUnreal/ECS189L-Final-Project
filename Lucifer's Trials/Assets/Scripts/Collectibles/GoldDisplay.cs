using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    public TMP_Text goldText;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.goldText.text = this.player.GetComponent<PlayerController>().GetWallet().ToString();
        //this.GetComponent<Text>().text = this.player.GetComponent<PlayerController>().GetWallet().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        this.goldText.text = this.player.GetComponent<PlayerController>().GetWallet().ToString();
        //this.GetComponent<Text>().text = this.player.GetComponent<PlayerController>().GetWallet().ToString();
    }
}