using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP : MonoBehaviour
{
    [SerializeField] int SPGain = 1;
    [SerializeField] bool despawnable = true;
    private float duration = 10.0f;
    private float curDuration;
    Func<bool> PlayerAtMaxSP;

    // Start is called before the first frame update
    void Start()
    {
        curDuration = 0.0f;

        var player = GameObject.Find("Player");
        PlayerAtMaxSP = GameObject.Find("Player").GetComponent<PlayerController>().AtMaxSP;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (!PlayerAtMaxSP())
            {
                Destroy(this.gameObject);
                // Increase player's SP
                col.GetComponent<PlayerController>().IncreaseSP(SPGain);
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