using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BombCooldownBar : MonoBehaviour
{
    public Image coolDownImage;
    private PlayerAction playerAction;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        coolDownImage.fillAmount = 0.0f;
        playerAction = GetComponentInParent<PlayerAction>();
        waitTime = playerAction.bombCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAction.canBomb == false)
        {
            //Show cooldown bar
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //Increase fill amount over BombCooldown timer
            coolDownImage.fillAmount += 1.0f / waitTime * Time.deltaTime;
        }
        else
        {
            //Set bar to 0
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            coolDownImage.fillAmount = 0.0f;
        }
    }
}
