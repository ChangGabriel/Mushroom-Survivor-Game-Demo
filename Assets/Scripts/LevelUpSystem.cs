using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    [SerializeField] private int level = 1;
    public float currentExp { get; private set; } = 0;
    private float requiredExp;
    private PlayerAction playerAction;
    private PlayerController playerController;

    [SerializeField] private GameObject levelUpFXPrefab;

    private void Start()
    {
        playerAction = GetComponent<PlayerAction>();
        playerController = GetComponent<PlayerController>();
    }

    private int calculateRequiredExp(int currentLevel)
    {
        if (currentLevel == 0)
        {
            return 0;
        }
        else
        {   //exp level formula
            return (currentLevel + currentLevel * 5);
        }
    }

    public void giveExp(float exp)
    {
        currentExp += exp;
        requiredExp = calculateRequiredExp(level);

        //Debug.Log("Exp: " + currentExp + " / " + "Req: " + requiredExp);
        if (currentExp >= requiredExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        //Increase Level
        level++;
        //damage scaling related
        playerAction.setSlashDamagelvl(1.10f);//input are percentage increase in damage ex. 1.10 is 10% increase
        playerAction.setBombDamagelvl(1.10f);
        playerAction.setAxeDamagelvl(1.10f);

        //Handles upgrades for the three attacks. Upgrades unlocks at different levels
        switch (level)
        {
            case 2:
                playerAction.setSlashNumber(2);     //enable slash burst
                playerAction.enableBombGroundDOT();
                break;
            case 3:
                playerAction.increaseAxeSize(1.5f); //Increase size the axe by percentage in decimal form
                break;
            case 4:
                playerAction.enableOppositeSlash(); //enable slash spawning in the opposite position of the original slash
                break;
            case 5:
                playerAction.enableBombGroundDOT(); //enable bomb to spawn a ground DOT effect after exploding
                break;
            case 6:
                playerAction.setSlashNumber(3);
                break;
            case 7:
                playerAction.enableCrossAxes();     //enable additonal axes to be throwned in a cross pattern
                break;
            case 8:
                playerAction.increaseAoeBomb(1.8f); //Increase size AOE of Bomb by percentage in decimal form
                break;
            case 9:
                playerAction.increaseAxeSize(2.0f);
                break;
            default:
                break;
        }

        //Modify Exp values
        currentExp -= requiredExp;
        Debug.Log("Level:" + level);
        //Spawn Levelup FX
        GameObject levelupFX = Instantiate(levelUpFXPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        levelupFX.transform.parent = this.transform;

    }



}
