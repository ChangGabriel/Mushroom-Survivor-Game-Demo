using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpImage;
    public Image hpEffectImage;
    private float effectDecreaseSpeed = 0.005f;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        hpImage.fillAmount = playerController.currentHealth / playerController.maxHealth;
        if(hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= effectDecreaseSpeed;
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
        if(playerController.currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
