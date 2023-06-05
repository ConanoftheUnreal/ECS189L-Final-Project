using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class Attackables : MonoBehaviour
{
    private float knockbackForce = 3.0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerHurtbox")
        {
            // damage set here as default; can be an attribute of the EnemyProjectile game object in general
            int damage = 1;
            col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.CQC);
            // get `Player` gameobject from collider of `PlayerHurtbox` and hurt player
            col.transform.parent.GetComponent<PlayerController>().DecreaseHealth(damage);
        }

        if (col.tag == "PlayerAttack")
        {
            var rb = this.GetComponent<Rigidbody2D>();

            var positionSelf = (Vector2)this.gameObject.transform.position;
            var positionAttack = col.ClosestPoint(this.gameObject.transform.position);
            var knockbackDirection = (positionSelf - positionAttack).normalized;

            rb.velocity = knockbackForce * knockbackDirection;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
