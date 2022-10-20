using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerAction : MonoBehaviour
{
    //Connected objects
    [SerializeField] private Transform attackPoint;
    private SkeletonAnimation skeletonAnimation;
    private PlayerController playerController;
    private Vector3 difference;
    public bool isAttacking;

    //Slash attack related
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float slashDamage;
    [SerializeField] private float slashCooldown;
    private bool canSlash;
    private int numberOfSlashes; //default:1, this increases with levels
    private bool oppositeSlash = false;
    
    //Axe throw related
    [SerializeField] private GameObject axePrefab;
    [SerializeField] private float axeDamage; //axe rotation speed when thrown
    [SerializeField] private float projectileSpeed;
    private GameObject axe;
    private GameObject CrossAxeRight;
    private GameObject CrossAxeBack;
    private GameObject CrossAxeLeft;
    private float axeSizeMulti = 1f;
    private Vector3 targetPos; //where we want to the axe to reach
    private Vector3 axeStartPos;
    private bool isThrown;
    private bool canCallBack;
    private bool axeReturn;
    private bool axeTimerStarted;
    private bool CrossAxes;
    private bool crossAxeSpawned;

    //Bomb Spell related
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject bombGroundDotPrefab;
    [SerializeField] private float bombDamage;
    [SerializeField] public float bombCooldown;
    [SerializeField] private float groundDotDamage;
    private float bombAoeSizeMulti = 1f;
    public bool canBomb { get; private set; }
    private bool groundDot = false;

    // Start is called before the first frame update
    void Start()
    {
        CrossAxes = false;
        canSlash = true;
        numberOfSlashes = 1;
        canBomb = true;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        playerController = GetComponent<PlayerController>();
        isAttacking = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isAlive == true && !PauseMenu.gameIsPaused)
        {
            aim();
            //slash attack
            if (Input.GetMouseButtonDown(0) && canSlash)
            {
                StartCoroutine(slashCooldownTimer());
                canSlash = false;
                isAttacking = true;
                StartCoroutine(isAttackingTimer());
                playerFaceDirection();
                StartCoroutine(SlashBurst(numberOfSlashes, 300)); ; //Slash Attack, first param: number of slashes, second param: rate of attack per minute
            }

            //Bomb Spell
            if (Input.GetKeyDown("e") && canBomb)
            {
                StartCoroutine(bombCooldownTimer());
                canBomb = false;
                isAttacking = true;
                StartCoroutine(isAttackingTimer());
                playerFaceDirection();
                bombAttack();
            }
            //Axe Throw
            if (Input.GetMouseButtonDown(1) && !isThrown)
            {
                isThrown = true;
                isAttacking = true;
                axeTimerStarted = false;
                StartCoroutine(isAttackingTimer());
                playerFaceDirection();
                if (!canCallBack)
                {
                    targetPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0); //where mouse is when clicked
                    axeStartPos = attackPoint.transform.position;
                    spawnAxe();
                }
            }
            if (isThrown)
            {
                axeThrow();
            }
        }
    }

    //Handle aiming, shall follow where mousepointer is
    private void aim()
    {
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - attackPoint.transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        attackPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    //Handles the players facing direction when attacking (attack up or down change animation acordingly)
    void playerFaceDirection()
    {
        Vector3 cameraPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (cameraPoint.y - transform.position.y <= 0)
        {
            skeletonAnimation.AnimationName = "Run";
        }
        else
        {
            skeletonAnimation.AnimationName = "RunB";

        }
    }

    //handles cooldown for Slash attack
    private IEnumerator slashCooldownTimer()
    {
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }

    //Handles the spawning of slashes
    private IEnumerator SlashBurst(int slashNumber, float rateOfattack)
    {
        float slashDelay = 60 / rateOfattack;
        // rate of attack is in attacks per minute (RPM), therefore we should calculate how much time passes before slashing again in the same burst.

        // Handle Spawn position for the slashes
        Vector3 spawnPos =  new Vector3(difference.x, difference.y, 0).normalized;
        Quaternion spawnRot = attackPoint.transform.rotation;

        for (int i = 0; i < slashNumber; i++)
        {
            GameObject slashSpawn = Instantiate(slashPrefab, attackPoint.transform.position + spawnPos, spawnRot); // It would be wise to use the gun barrel's position and rotation to align the bullet to.
            slashSpawn.GetComponent<Slash>().setSlashDamage(slashDamage);
            slashSpawn.GetComponent<Slash>().playerPos = attackPoint.transform.position;

            // Handle Slash reverse spawn (Level up upgrade)
            if (oppositeSlash)
            {
                GameObject slashSpawnBack = Instantiate(slashPrefab, attackPoint.transform.position - spawnPos, spawnRot * Quaternion.Euler(0, 0, -180));
                slashSpawnBack.GetComponent<Slash>().setSlashDamage(slashDamage);
                slashSpawnBack.GetComponent<Slash>().playerPos = attackPoint.transform.position;
            }
            yield return new WaitForSeconds(slashDelay); // wait till the next attack
        }
    }
    //Handles bomb spell spawning
    private void bombAttack()
    {
        // Handle Bomb spell spawn
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        GameObject bombSpawn = Instantiate(bombPrefab, pos, Quaternion.identity);
        bombSpawn.transform.localScale = bombSpawn.transform.localScale * bombAoeSizeMulti;
        bombSpawn.GetComponent<BombSpell>().setBombDamage(bombDamage);
        if (groundDot)
        {
            GameObject groundDotSpawn = Instantiate(bombGroundDotPrefab, pos, Quaternion.identity);
            groundDotSpawn.GetComponent<BombGroundDOT>().setBombGroundDamage(groundDotDamage);
        }
    }

    //handles cooldown for bomb spell
    private IEnumerator bombCooldownTimer()
    {
        yield return new WaitForSeconds(bombCooldown);
        canBomb = true;
    }

    private void spawnAxe()
    {
        // Handle aiming, shall follow where mousepointer is
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - attackPoint.transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        attackPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        // Handles axe projectile spawn
        axe = Instantiate(axePrefab, attackPoint.transform.position + new Vector3(difference.x, difference.y, 0).normalized, attackPoint.transform.rotation);
        axe.transform.localScale = axe.transform.localScale * axeSizeMulti;
        axe.GetComponent<AxeThrow>().setAxeDamage(axeDamage);
        axe.GetComponent<AxeThrow>().muted = false;
        // Handle cross axe spawns
        if (CrossAxes)
        {
            crossAxeSpawned = true;
            //Using matrix multiplication to rotate the spawn position
            CrossAxeRight = Instantiate(axePrefab, attackPoint.transform.position + new Vector3(difference.y, -difference.x, 0).normalized, attackPoint.transform.rotation * Quaternion.Euler(0, 0, -90));
            CrossAxeRight.transform.localScale = CrossAxeRight.transform.localScale * axeSizeMulti;
            CrossAxeRight.GetComponent<AxeThrow>().setAxeDamage(axeDamage);

            CrossAxeBack = Instantiate(axePrefab, attackPoint.transform.position - new Vector3(difference.x, difference.y, 0).normalized, attackPoint.transform.rotation * Quaternion.Euler(0, 0, -180));
            CrossAxeBack.transform.localScale = CrossAxeBack.transform.localScale * axeSizeMulti;
            CrossAxeBack.GetComponent<AxeThrow>().setAxeDamage(axeDamage);

            CrossAxeLeft = Instantiate(axePrefab, attackPoint.transform.position + new Vector3(-difference.y, difference.x, 0).normalized, attackPoint.transform.rotation * Quaternion.Euler(0, 0, -270));
            CrossAxeLeft.transform.localScale = CrossAxeLeft.transform.localScale * axeSizeMulti;
            CrossAxeLeft.GetComponent<AxeThrow>().setAxeDamage(axeDamage);

        }
    }

    private void axeThrow()
    {
        // Handle axe movement and booleans
        axe.GetComponent<AxeThrow>().activateAxe();
        if (!canCallBack) //move towards target if axe has not been thrown
        {
            axe.transform.position = Vector2.MoveTowards(axe.transform.position, targetPos, projectileSpeed * Time.deltaTime);
            if (crossAxeSpawned) //Handle cross axe upgrade (Movement)
            {
                Vector3 difference = targetPos - axeStartPos;
                CrossAxeRight.GetComponent<AxeThrow>().activateAxe();
                CrossAxeBack.GetComponent<AxeThrow>().activateAxe();
                CrossAxeLeft.GetComponent<AxeThrow>().activateAxe();
                CrossAxeRight.transform.position = Vector2.MoveTowards(CrossAxeRight.transform.position, axeStartPos + new Vector3(difference.y, -difference.x, 0), projectileSpeed * Time.deltaTime);
                CrossAxeBack.transform.position = Vector2.MoveTowards(CrossAxeBack.transform.position, axeStartPos - new Vector3(difference.x, difference.y, 0), projectileSpeed * Time.deltaTime);
                CrossAxeLeft.transform.position = Vector2.MoveTowards(CrossAxeLeft.transform.position, axeStartPos + new Vector3(-difference.y, difference.x, 0), projectileSpeed * Time.deltaTime);
            }
        }
        if(canCallBack && axeReturn)
        {
            axe.transform.position = Vector2.MoveTowards(axe.transform.position, attackPoint.transform.position, projectileSpeed * 4 * Time.deltaTime);

            if (crossAxeSpawned)
            {
                CrossAxeRight.GetComponent<AxeThrow>().activateAxe();
                CrossAxeBack.GetComponent<AxeThrow>().activateAxe();
                CrossAxeLeft.GetComponent<AxeThrow>().activateAxe();
                CrossAxeRight.transform.position = Vector2.MoveTowards(CrossAxeRight.transform.position, attackPoint.transform.position, projectileSpeed* 4 * Time.deltaTime);
                CrossAxeBack.transform.position = Vector2.MoveTowards(CrossAxeBack.transform.position, attackPoint.transform.position, projectileSpeed* 4 * Time.deltaTime);
                CrossAxeLeft.transform.position = Vector2.MoveTowards(CrossAxeLeft.transform.position, attackPoint.transform.position, projectileSpeed* 4 * Time.deltaTime);
            }
            
        }
        
        // Handle when axe has reach targeted pos, No damage and rotation
        if (Vector2.Distance(axe.transform.position, targetPos) <= 0.01f)
        {
            if (!axeTimerStarted)
            {
                StartCoroutine(axeReturnTimer()); //start timer for axe return
                axeTimerStarted = true;
                axe.GetComponent<AxeThrow>().mute(true);
            }
            canCallBack = true;
            axe.GetComponent<AxeThrow>().resetAxe();

            if (crossAxeSpawned)
            {
                CrossAxeRight.GetComponent<AxeThrow>().resetAxe();
                CrossAxeBack.GetComponent<AxeThrow>().resetAxe();
                CrossAxeLeft.GetComponent<AxeThrow>().resetAxe();
            }
        }
        // Handle when axe(axes) has reached back to player
        if (crossAxeSpawned)
        {
            if((Vector2.Distance(axe.transform.position, attackPoint.transform.position) <= 0.01f) && 
               (Vector2.Distance(CrossAxeRight.transform.position, attackPoint.transform.position) <= 0.01f) &&
               (Vector2.Distance(CrossAxeBack.transform.position, attackPoint.transform.position) <= 0.01f) && 
               (Vector2.Distance(CrossAxeLeft.transform.position, attackPoint.transform.position) <= 0.01f))
            {
                Destroy(CrossAxeRight);
                Destroy(CrossAxeBack);
                Destroy(CrossAxeLeft);
                Destroy(axe);
                isThrown = false;
                canCallBack = false;
                axeReturn = false;
            }  
        }else if(Vector2.Distance(axe.transform.position, attackPoint.transform.position) <= 0.01f)
        {
            isThrown = false;
            canCallBack = false;
            axeReturn = false;
            Destroy(axe);
        }
    }

    //handles timer for axe throw return
    private IEnumerator axeReturnTimer()
    {
        yield return new WaitForSeconds(0.15f);
        axeReturn = true;
    }

    //handles timer for boolean isAttacking
    private IEnumerator isAttackingTimer()
    {
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
    
    //Level up related methods
    public void setSlashDamagelvl(float percentageDmgInc)
    {
        slashDamage = Mathf.RoundToInt(slashDamage * percentageDmgInc); //ex. 100 * 1.10 (10% increase)
    }

    public void setSlashNumber(int slashNumber)
    {
        numberOfSlashes = slashNumber;
    }

    public void enableOppositeSlash()
    {
        oppositeSlash = true;
    }
    public void setBombDamagelvl(float percentageDmgInc)
    {
        bombDamage = Mathf.RoundToInt(bombDamage * percentageDmgInc);
    }
    public void increaseAoeBomb(float percentageIncrease)
    {
        bombAoeSizeMulti = percentageIncrease;
    }

    public void enableBombGroundDOT()
    {
        groundDot = true;
    }
    public void setAxeDamagelvl(float percentageDmgInc)
    {
        axeDamage = Mathf.RoundToInt(axeDamage * percentageDmgInc);
    }
    public void enableCrossAxes()
    {
        CrossAxes = true;
    }
    public void increaseAxeSize(float percentageIncrease)
    {
        axeSizeMulti = percentageIncrease;
    }
}
