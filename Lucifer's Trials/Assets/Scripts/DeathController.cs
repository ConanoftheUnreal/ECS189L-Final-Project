using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public void DeathDisappear()
    {
        this.gameObject.transform.Find("Hitbox").gameObject.GetComponent<AttackableController>().DeathDisappear();
        Destroy(this.gameObject);
    }
}