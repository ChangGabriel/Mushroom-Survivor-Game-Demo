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

        Debug.Log("Exp: " + currentExp + " / " + "Req: " + requiredExp);
        if (currentExp >= requiredExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        //Modify stats
        level++;

        //Modify Exp values
        currentExp -= requiredExp;
        Debug.Log("Level:" + level);
        //Spawn Levelup FX
        GameObject levelupFX = Instantiate(levelUpFXPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        levelupFX.transform.parent = this.transform;

    }



}
