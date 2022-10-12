using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGroundDOT : MonoBehaviour
{
    private float damage;
    private float damageRange;
    [SerializeField] private float damageRangeProcentage;
    [SerializeField] private float damageFrequency;
    private Collider2D enemyHit;
    [SerializeField] private float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BombGroundDotTimer());
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //damage
            damageRange = Random.Range((damage - damage * damageRangeProcentage), (damage + damage * damageRangeProcentage));
            other.GetComponent<Enemy>().takeDamage(damageRange, false);
        }
    }

    private IEnumerator BombGroundDotTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageFrequency/2);
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(damageFrequency/2);
            GetComponent<Collider2D>().enabled = true;
        }
    }

    public void setBombGroundDamage(float damage)
    {
        this.damage = damage;
    }
}
