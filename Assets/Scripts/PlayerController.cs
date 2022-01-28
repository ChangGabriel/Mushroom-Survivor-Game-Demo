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
    private bool isAttacking; //from playerAction script

    // Player stats
    [SerializeField] private float moveSpeed;
    public float maxHealth;
    [HideInInspector] public float currentHealth;

    /* Experience and level related
    private int level;
    private int currentExp = 0;
    private int requiredExp = 10; 
    */

    // Material related
    protected Material matWhite;
    protected Material matDefault;
    private MaterialPropertyBlock mpb;

    // Hidden variables in inspector
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private bool isAlive;
    private bool isHurt;



    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        isHurt = false;
        playerRB = GetComponent<Rigidbody2D>();
        mpb = new MaterialPropertyBlock();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            move();
        }
    }

    //Handles the players movement
    private void move()
    {
        //Debug.Log(movement.y);
        isAttacking = GetComponent<PlayerAction>().isAttacking;

        movement.x = Input.GetAxisRaw("Horizontal"); //when moving right movement.x = 1 and left movement.x = -1
        movement.y = Input.GetAxisRaw("Vertical");
        playerRB.MovePosition(playerRB.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        if(!isHurt && !isAttacking && movement.normalized == Vector2.zero)
        {
            skeletonAnimation.AnimationName = "Idle";
        }
        if (!isHurt && !isAttacking && !(movement.normalized == Vector2.zero) && movement.y <= 0)
        {
            skeletonAnimation.AnimationName = "Run";
        }
        else if(!isHurt && !isAttacking && !(movement.normalized == Vector2.zero))
        {
            skeletonAnimation.AnimationName = "RunB";

        }
    }

    //Handles player taking damage
    public void playerTakeDamage(float damage)
    {
        currentHealth -= damage;
        //Boolean for hurt animation
        isHurt = true;
        skeletonAnimation.AnimationName = "Hurt";
        StartCoroutine(hurtTimer());
        mpb.SetFloat("_FillPhase", 1.0f);
        GetComponent<MeshRenderer>().SetPropertyBlock(mpb);

        if (currentHealth < 0)
        {
            currentHealth = 0; //stops UI from displaying negative HP
        }
        if (currentHealth <= 0)
        {
            isAlive = false;
            playerRB.velocity = Vector2.zero;
            GetComponent<CapsuleCollider2D>().enabled = false;
            mpb.SetFloat("_FillPhase", 1.0f);
            GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
            StartCoroutine(die());
        }
        else
        {
            Invoke("resetMat", 0.1f);
        }
    }

    //Handle player death
    private IEnumerator die()
    {
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart to current scene

    }

    //Resets the material to default
    private void resetMat()
    {
        if (isAlive)
        {
            //spriteRenderer.material = matDefault;
            mpb.SetFloat("_FillPhase", 0.0f);
            GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
        }
    }

    //handles cooldown for hurt boolean
    public IEnumerator hurtTimer()
    {
        float hurtDuration = skeletonAnimation.skeleton.Data.FindAnimation("Hurt").Duration; //0.6667 sec
        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
    }
}
