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
    private MeshRenderer mRenderer;
    private bool isAttacking; //from playerAction script
    [SerializeField] GameObject stepAudioGameObject;

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
    public bool isAlive { get; private set; }
    private bool isHurt;
    private AudioSource[] playerAudioSources; // this is a list of different auido sources connected to the player. [0] refers to step sound effect. [1] refers to get hit SFX


    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        isHurt = false;
        playerRB = GetComponent<Rigidbody2D>();
        mpb = new MaterialPropertyBlock();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        mRenderer = GetComponent<MeshRenderer>();
        currentHealth = maxHealth;
        playerAudioSources = stepAudioGameObject.GetComponents<AudioSource>();
        playerAudioSources[0].mute = true;

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
            playerAudioSources[0].mute = true;
        }
        if (!isHurt && !isAttacking && !(movement.normalized == Vector2.zero) && movement.y <= 0)
        {
            skeletonAnimation.AnimationName = "Run";
            playerAudioSources[0].mute = false;
            playerAudioSources[0].pitch = Random.Range(0.8f, 1.4f);
        }
        else if(!isHurt && !isAttacking && !(movement.normalized == Vector2.zero))
        {
            skeletonAnimation.AnimationName = "RunB";
            playerAudioSources[0].mute = false;
            playerAudioSources[0].pitch = Random.Range(0.8f, 1.4f);

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

        //play hit sound
        playerAudioSources[1].Play();

        if (currentHealth < 0)
        {
            currentHealth = 0; //stops UI from displaying negative HP
        }
        if (currentHealth <= 0)
        {
            //Disable related options
            isAlive = false;
            playerAudioSources[0].mute = true;
            playerRB.velocity = Vector2.zero;
            GetComponent<CapsuleCollider2D>().enabled = false;
            mpb.SetFloat("_FillPhase", 1.0f);
            mRenderer.SetPropertyBlock(mpb);
            mRenderer.enabled = false;
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
