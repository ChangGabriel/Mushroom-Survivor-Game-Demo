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
            case 3:
                playerAction.enableOppositeSlash(); //enable slash spawning in the opposite position of the original slash
                break;
            case 4:
                playerAction.setSlashNumber(3); //enable slash burst
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
