using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Connected objects
    private Rigidbody2D playerRB;
    [SerializeField] private Transform shadow;
    private SkeletonAnimation skeletonAnimation;


    // Player stats
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    // Hidden variables in inspector
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        //spriteRenderer = transform.GetComponent<SpriteRenderer>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        currentHealth = maxHealth;


    }

    // Update is called once per frame
    void Update()
    {
        //playerFaceDirection();

    }

    private void FixedUpdate()
    {
        move();
    }

    //Handles the players facing direction (attack up or down change animation acordingly)
    void playerFaceDirection()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //when moving right movement.x = 1 and left movement.x = -1
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
            shadow.localPosition = new Vector2(-0.02f, 0);
        }
        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
            shadow.localPosition = new Vector2(0.02f, 0);
        }
    }

    //Handles the players movement
    private void move()
    {
        //Debug.Log(movement.y);

        movement.x = Input.GetAxisRaw("Horizontal"); //when moving right movement.x = 1 and left movement.x = -1
        movement.y = Input.GetAxisRaw("Vertical");
        playerRB.MovePosition(playerRB.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        if(movement.normalized == Vector2.zero)
        {
            skeletonAnimation.AnimationName = "Idle";
        }
        if (!(movement.normalized == Vector2.zero) && movement.y <= 0)
        {
            skeletonAnimation.AnimationName = "Run";
        }
        else if(!(movement.normalized == Vector2.zero))
        {
            skeletonAnimation.AnimationName = "RunB";

        }
    }

    //Handles player taking damage
    public void playerTakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0; //stops UI from displaying negative HP
        }
        if (currentHealth <= 0)
        {
            playerRB.velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(die());
        }
    }

    //Handle player death
    private IEnumerator die()
    {
        float ticks = 10f;
        for (int i = 1; i < ticks + 1; i++)
        {
            spriteRenderer.material.SetFloat("_Fade", 1 - i / ticks);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(7.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart to current scene

    }
}
