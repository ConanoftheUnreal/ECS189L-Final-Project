using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lucifer;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] int damage = 3;
    [SerializeField] ProjectileTypes projectileType;
    [SerializeField] GameObject afterEffectPrefab;
    private float speed = 12.0f;

    public void OnTriggerEnter2D(Collider2D col)
    {
        // Sorceress' fireball breaks on impact with explosion sound
        if (this.tag == "Projectile" && (col.tag == "EnemyHurtbox" || col.tag == "Wall"))
        {
            Destroy(this.gameObject);
            AfterEffect();
            FindObjectOfType<SoundManager>().PlaySoundEffect("Explosion");
        }
        // All projectiles that hit a wall should be destroyed.
        else if ( (col.tag == "Wall") || (col.tag == "PlayerAttack") )
        {
            Destroy(this.gameObject);
            AfterEffect();
            FindObjectOfType<SoundManager>().PlaySoundEffect("Tink");
        }

        // Enemy projectiles that hit the player get destroyed
        if (this.tag == "EnemyProjectile" && col.tag == "PlayerHurtbox")
        {
            // get `Player` gameobject from collider of `PlayerHurtbox` and hurt player
            bool playerHit = col.transform.parent.gameObject.GetComponent<PlayerAnimationController>().PlayerDamaged(this.gameObject, damage, DamageTypes.RANGED);
            if (playerHit)
            {
                Destroy(this.gameObject);
                AfterEffect();
                FindObjectOfType<SoundManager>().PlaySoundEffect("Player Hurt");
                FindObjectOfType<SoundManager>().PlaySoundEffect("Tink");
            }
        }
    }

    private void AfterEffect()
    {
        GameObject effect;
        switch (projectileType)
        {
            case ProjectileTypes.FIRE:
                // ignition (burn spot) effect
                effect = Instantiate(this.afterEffectPrefab, this.gameObject.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                break;
            case ProjectileTypes.PHYSICAL:
                // break (pop) effect
                effect = Instantiate(this.afterEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
                break;
            default:
                Debug.Log("Error: Invalid projectile type.");
                break;
        }
    }

    public float GetProjectileSpeed()
    {
        return this.speed;
    }

    public void SetProjectileDamage(int dmg)
    {
        this.damage = dmg;
    }

}
