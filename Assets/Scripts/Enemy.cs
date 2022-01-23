using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Enemy stats
    [SerializeField] private string enemyName;
    [SerializeField] protected private float moveSpeed;
    [SerializeField] protected private float maxHealth;
    [SerializeField] private float health;
    private bool isAlive;

    // Knockback and stun Related
    [HideInInspector]
    public bool canBeKnockedBack;
    [HideInInspector]
    public bool isStunned;
    private float knockBackForce = 10f;
    private float knockBackCooldown = 0.2f;

    // Material related
    protected Material matWhite;
    protected Material matDeath;
    protected Material matDefault;

    // Connected objects
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D enemyRigidbody;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject damageCanvas;

    // Aggro related
    [SerializeField] protected private Transform target;
    [SerializeField] protected private float aggroDistance;
    [SerializeField] protected private float attackRange;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        canBeKnockedBack = true;
        isAlive = true;
        enemyRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("Materials/White-Flash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;


        // Target player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        facePlayer();
    }

    private void FixedUpdate()
    {
        if (isAlive && !isStunned)
        {
            move();
        }
    }

    protected virtual void attack()
    {
        //does nothing now. For future cases.
    }

    protected virtual void move()
    {   //move towards player if within aggro distance
        if (Vector2.Distance(transform.position, target.position) <= aggroDistance && !(Vector2.Distance(transform.position, target.position) <= attackRange))
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

    }

    protected virtual void facePlayer()
    {
        //Enemy faces player by fliping the sprite
        if ((Vector2.Distance(transform.position, target.position) <= aggroDistance) && transform.position.x < target.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (Vector2.Distance(transform.position, target.position) <= aggroDistance)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void takeDamage(float damage)
    {
        if (isAlive)
        {
            //Damage number UI related
            DamageNumber damageNum = Instantiate(damageCanvas, transform.position, Quaternion.identity).GetComponent<DamageNumber>();
            damageNum.setUIDamage(Mathf.RoundToInt(damage)); 

            health -= damage;
            spriteRenderer.material = matWhite;

            if (health <= 0)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                isAlive = false;
                enemyRigidbody.velocity = Vector2.zero;
                die();
            }
            else
            {
                Invoke("resetMat", 0.1f);
            }
        }
        
    }

    //Resets the material to default
    private void resetMat()
    {
        if (isAlive)
        {
            spriteRenderer.material = matDefault;

        }
    }
    private void die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //handles cooldown for knockback
    public IEnumerator knockbackTimer()
    {
        yield return new WaitForSeconds(knockBackCooldown);
        canBeKnockedBack = true;
    }

    //Handles the knockback of this enemy gameObject
    public void knockback(Vector2 knockBackDirection)
    {
        Vector2 direction = knockBackDirection;
        Vector2 force = direction * knockBackForce;
        enemyRigidbody.AddForce(force, ForceMode2D.Impulse);
        canBeKnockedBack = false;
        StartCoroutine(knockbackTimer());
    }

    //handles cooldown for stun
    public IEnumerator stunTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
    public void stun(float duration)
    {
        isStunned = true;
        StartCoroutine(stunTimer(duration));
    }
}
