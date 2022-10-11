using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerAction : MonoBehaviour
{
    //Connected objects
    [SerializeField] private Transform attackPoint;
    private SkeletonAnimation skeletonAnimation;
    private Vector3 difference;
    public bool isAttacking;

    //Slash attack related
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float slashDamage;
    [SerializeField] private float slashCooldown;
    private bool canSlash;
    
    //Axe throw related
    [SerializeField] private GameObject axePrefab;
    [SerializeField] private float axeDamage; //axe rotation speed when thrown
    [SerializeField] private float projectileSpeed;
    private GameObject axe;
    private Vector3 targetPos; //where we want to the axe to reach
    private bool isThrown;
    private bool canCallBack;

    //Bomb Spell related
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombDamage;
    [SerializeField] private float bombCooldown;
    private bool canBomb;


    // Start is called before the first frame update
    void Start()
    {
        canSlash = true;
        canBomb = true;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        isAttacking = false;

    }

    // Update is called once per frame
    void Update()
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
            slashAttack(); //Slash Attack
        }

        //Bomb Spell
        if(Input.GetKeyDown("e") && canBomb)
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
            StartCoroutine(isAttackingTimer());
            playerFaceDirection();
            if (!canCallBack)
            {
                targetPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0); //where mouse is when clicked
                spawnAxe();
            }
        }
        if (isThrown)
        {
            axeThrow();
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
    private void slashAttack()
    {

        // Handle Slash projection spawn
        GameObject slashSpawn = Instantiate(slashPrefab, attackPoint.transform.position + new Vector3(difference.x, difference.y, 0).normalized, attackPoint.transform.rotation);
        slashSpawn.GetComponent<Slash>().setSlashDamage(slashDamage);
        slashSpawn.GetComponent<Slash>().playerPos = attackPoint.transform.position;

    }

    //handles cooldown for Slash attack
    private IEnumerator slashCooldownTimer()
    {
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }

    private void bombAttack()
    {

        // Handle Bomb spell spawn
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        GameObject bombSpawn = Instantiate(bombPrefab, pos, Quaternion.identity);
        bombSpawn.GetComponent<BombSpell>().setBombDamage(slashDamage);

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
        axe.GetComponent<AxeThrow>().setPlayerPos(attackPoint.transform.position);
        axe.GetComponent<AxeThrow>().setAxeDamage(axeDamage);
    }

    private void axeThrow()
    {
        // Handle axe movement and booleans
        axe.GetComponent<AxeThrow>().setcanDamage(true);
        axe.GetComponent<AxeThrow>().setisRotating(true);
        if (!canCallBack) //move towards target if axe has not been thrown
        {
            axe.transform.position = Vector2.MoveTowards(axe.transform.position, targetPos, projectileSpeed * Time.deltaTime);
        }
        else
        {
            axe.transform.position = Vector2.MoveTowards(axe.transform.position, attackPoint.transform.position, projectileSpeed * 4 * Time.deltaTime);
        }
        // Handle when axe has reach targeted pos, No damage and rotation
        if (Vector2.Distance(axe.transform.position, targetPos) <= 0.01f)
        {
            isThrown = false;
            canCallBack = true;
            axe.GetComponent<AxeThrow>().setcanStun(false);
            axe.GetComponent<AxeThrow>().setisRotating(false);
            axe.GetComponent<AxeThrow>().setcanDamage(false);
            axe.GetComponent<AxeThrow>().enemyHit = null;
        }
        // Handle when axe has reached back to player
        if (Vector2.Distance(axe.transform.position, attackPoint.transform.position) <= 0.01f)
        {
            isThrown = false;
            canCallBack = false;
            Destroy(axe);
        }
    }

    //handles timer for boolean isAttacking
    private IEnumerator isAttackingTimer()
    {
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
    
    public void setSlashDamagelvl(float percentageDmgInc)
    {
        slashDamage = Mathf.RoundToInt(slashDamage * percentageDmgInc); //ex. 100 * 1.10 (10% increase)
    }
    public void setBombDamagelvl(float percentageDmgInc)
    {
        bombDamage = Mathf.RoundToInt(bombDamage * percentageDmgInc);
    }
    public void setAxeDamagelvl(float percentageDmgInc)
    {
        axeDamage = Mathf.RoundToInt(axeDamage * percentageDmgInc);
    }
}
