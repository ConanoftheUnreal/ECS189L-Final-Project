using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool despawnable = true;
    private float duration = 10.0f;
    private float curDuration;
    Func<bool> PlayerAtMaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        curDuration = 0.0f;

        var player = GameObject.Find("Player");
        PlayerAtMaxHealth = GameObject.Find("Player").GetComponent<PlayerController>().AtMaxHealth;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (!PlayerAtMaxHealth())
            {
                Destroy(this.gameObject);
                // Increase player's health
                FindObjectOfType<SoundManager>().PlaySoundEffect("Item Pickup");
                var healthGain = (int)(col.GetComponent<PlayerController>().GetMaxHealth() * 0.20f);   // give 20% health
                col.GetComponent<PlayerController>().IncreaseHealth(healthGain);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (despawnable)
        {
            if (curDuration > duration)
            {
                Destroy(this.gameObject);
            }
            curDuration += Time.deltaTime;
        }
    }
}
