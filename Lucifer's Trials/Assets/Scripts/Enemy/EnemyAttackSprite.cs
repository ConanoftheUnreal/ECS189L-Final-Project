using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class EnemyAttackSprite : MonoBehaviour
{
    private int enemyDamage = 2;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerHurtbox")
        {
            col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, enemyDamage, DamageTypes.CQC);
        }
    }

}
