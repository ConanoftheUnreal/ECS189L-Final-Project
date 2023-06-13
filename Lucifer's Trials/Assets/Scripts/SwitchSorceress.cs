using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lucifer;

public class SwitchSorceress : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private PlayerStats playerStats;

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
        if (Input.GetKeyDown(KeyCode.E) && this.player != null)
        {
            this.player.GetComponent<PlayerAnimationController>().SetClass(PlayerType.SORCERESS);
            this.playerStats.playerType = PlayerType.SORCERESS;
            this.player.GetComponent<PlayerController>().newClass();
            this.player.GetComponent<PlayerAnimationController>().StartNewAnimation();
        }
    }
}
