using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private float damage;
    private float damageRange;
    [SerializeField] private float damageRangeProcentage;
    private Collider2D enemyHit;
    [SerializeField] private float lifetime;
    public Vector3 playerPos;


    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    public void endAttack()
    {
        //gameObject.SetActive(false);
        enemyHit = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy") && !(enemyHit == other))
        {
            enemyHit = other;
            //CinemachineShake.Instance.shakeCamera(3.5f, 0.1f); //shake camera if enemy is hit
            //Knockback and damage
            if (other.GetComponent<Enemy>().canBeKnockedBack)
            {
                Vector2 knockBackDir = (other.transform.position - playerPos).normalized; //direction of the slash
                other.GetComponent<Enemy>().knockback(knockBackDir);
            }
            damageRange = Random.Range((damage - damage * damageRangeProcentage), (damage + damage * damageRangeProcentage));
            other.GetComponent<Enemy>().takeDamage(damageRange);
        }
        
    }
    public void setSlashDamage(float damage)
    {
        this.damage = damage;
    }
}
