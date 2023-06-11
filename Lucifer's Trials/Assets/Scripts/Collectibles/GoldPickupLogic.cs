using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class GoldPickupLogic : MonoBehaviour
{
    [SerializeField] CurrencyTypes type = CurrencyTypes.COINS;
    [SerializeField] bool despawnable = true;
    private float duration = 10.0f;
    private float curDuration;

    // Start is called before the first frame update
    void Start()
    {
        curDuration = 0.0f;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Destroy(this.gameObject);

            // Add to player's Wallet
            int amount = 0;
            switch (this.type)
            {
                case CurrencyTypes.COINS:
                    amount = 10;
                    break;
                case CurrencyTypes.GOLD:
                    amount = 50;
                    break;
                case CurrencyTypes.GEMS:
                    amount = 100;
                    break;
                default:
                    Debug.Log("Error: Invalid currency type provided.");
                    break;
            }
            FindObjectOfType<SoundManager>().PlaySoundEffect("Coin Pickup");
            col.GetComponent<PlayerController>().IncreaseWallet(amount);
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