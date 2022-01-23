using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrow : MonoBehaviour
{
    private float damage;
    private float damageRange;
    [SerializeField] private float damageRangeProcentage;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float stunDuration;
    private Collider2D enemyHit;
    private Vector3 playerPos;
    private bool isRotating;
    private bool canDamage;
    private bool canStun;

    // Start is called before the first frame update
    void Start()
    {
        canStun = true;
    }

    // Update is called once per frame
    void Update()
    {
        selfRotate();
    }
    // Handle rotation of axe when thrown
    private void selfRotate()
    {
        if (isRotating)
        {
            transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && canDamage && !(enemyHit == other)) //axe can not damage when it has landed
        {

            enemyHit = other;
            CinemachineShake.Instance.shakeCamera(3.5f, 0.1f); //shake camera if enemy is hit
            //Knockback and damage
            if (!other.GetComponent<Enemy>().isStunned && canStun)
            {
                other.GetComponent<Enemy>().stun(stunDuration);
            }
            damageRange = Random.Range((damage - damage * damageRangeProcentage), (damage + damage * damageRangeProcentage));
            other.GetComponent<Enemy>().takeDamage(damageRange);
        }

    }

    public void setisRotating(bool rotate)
    {
        isRotating = rotate;
    }
    public void setcanDamage(bool i)
    {
        canDamage = i;
    }
    public void setcanStun(bool i)
    {
        canStun = i;
    }
    public void setAxeDamage(float damage)
    {
        this.damage = damage;
    }
    public void setPlayerPos(Vector3 pos)
    {
        playerPos = pos;
    }
}
