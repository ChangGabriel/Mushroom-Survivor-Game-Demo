using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Enemy
{
    // Frog Stats
    [SerializeField] private float foxDamage;
    [SerializeField] private float foxMoveSpeed;

    protected override void Start()
    {
        base.Start();
        moveSpeed = foxMoveSpeed;
    }

    //Handles player taking damage and player can't get hit more than once per attack
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canAttack)
            {
                other.GetComponent<PlayerController>().playerTakeDamage(foxDamage);
                canAttack = false;
                StartCoroutine(attackTimer());
            }

        }
    }
}
