using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
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
        Debug.Log("Collision");
        if (col.tag == "Player")
        {
            Destroy(this.gameObject);
            // Increase player's health
            col.GetComponent<PlayerController>().IncreaseHealth(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curDuration > duration)
        {
            Destroy(this.gameObject);
        }
        curDuration += Time.deltaTime;
    }
}
