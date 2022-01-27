using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    // Frog Stats
    [SerializeField] private float frogDamage;
    [SerializeField] private float frogMoveSpeed;

    protected override void Start()
    {
        base.Start();
        moveSpeed = frogMoveSpeed;
    }


    //Handles player taking damage and player can't get hit more than once per attack
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canAttack)
            {
                other.GetComponent<PlayerController>().playerTakeDamage(frogDamage);
                canAttack = false;
                StartCoroutine(attackTimer());
            }

        }
    }

    

}
