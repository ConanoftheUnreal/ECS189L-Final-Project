using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP : MonoBehaviour
{
    private float duration = 10.0f;
    private float curDuration;

    // Start is called before the first frame update
    void Start()
    {
        curDuration = 0.0f;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Collision");
        if (col.tag == "Player")
        {
            Destroy(this.gameObject);
            // Increase player's SP
            col.GetComponent<PlayerController>().IncreaseSP(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("ASD");
        if (curDuration > duration)
        {
            Destroy(this.gameObject);
        }
        curDuration += Time.deltaTime;
    }
}