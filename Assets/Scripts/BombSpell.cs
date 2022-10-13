using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpell : MonoBehaviour
{
    private float damage;
    private float damageRange;
    [SerializeField] private float damageRangeProcentage;
    [SerializeField] private float stunDuration;
    private Collider2D enemyHit;
    [SerializeField] private float lifetime;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(bombColliderTimer());
        Destroy(gameObject, lifetime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !(enemyHit == other))
        {
            enemyHit = other;
            //CinemachineShake.Instance.shakeCamera(3.5f, 0.1f); //shake camera if enemy is hit
            //Knockback and damage
            if (other.GetComponent<Enemy>().canBeKnockedBack && !other.GetComponent<Enemy>().isStunned)
            {
                Vector2 knockBackDir = (other.transform.position - transform.position).normalized; //direction of the slash
                other.GetComponent<Enemy>().knockback(knockBackDir);
                other.GetComponent<Enemy>().stun(stunDuration);
            }
            damageRange = Random.Range((damage - damage * damageRangeProcentage), (damage + damage * damageRangeProcentage));
            other.GetComponent<Enemy>().takeDamage(damageRange, true);
        }

    }
    public void setBombDamage(float damage)
    {
        this.damage = damage;
    }

    private IEnumerator bombColliderTimer()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider2D>().enabled = false;
    }
}
