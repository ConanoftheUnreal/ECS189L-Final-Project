using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class UpgradePickupLogic : MonoBehaviour
{
    [SerializeField] UpgradeTypes type = UpgradeTypes.ATTACK;
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

            switch (this.type)
            {
                case UpgradeTypes.MAXHEALTH:
                    col.GetComponent<PlayerController>().IncreaseMaxHealth(1);
                    break;
                case UpgradeTypes.ATTACK:
                    col.GetComponent<PlayerController>().IncreaseAttack(1);
                    break;
                default:
                    Debug.Log("Error: Invalid upgrade type provided.");
                    break;
            }
            FindObjectOfType<SoundManager>().PlaySoundEffect("Item Pickup");
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