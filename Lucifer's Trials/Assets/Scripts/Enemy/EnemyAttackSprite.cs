using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class EnemyAttackSprite : MonoBehaviour
{
    private GoblinBeserker self;
    private int damage;

    public void Start()
    {
        this.self = this.transform.parent.gameObject.GetComponent<GoblinBeserker>();
        this.damage = (int)self.Stats.Damage;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerHurtbox")
        {
            var wasHurt = col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, this.damage, DamageTypes.CQC);
            if (wasHurt)
            {
                FindObjectOfType<SoundManager>().PlaySoundEffect("Player Hurt");
            }
        }
    }

}
