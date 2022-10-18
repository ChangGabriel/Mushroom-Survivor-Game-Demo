using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifications : MonoBehaviour
{
    private TextMeshProUGUI textM;
    private Color textColor;


    private string levelUpText = "You have reached level ";
    private string deathText = "You have died. Restarting in 5 seconds...";

    // Start is called before the first frame update
    void Start()
    {
        textM = GetComponent<TextMeshProUGUI>();
        textColor = textM.color;
    }

    public void notifyLevelUp(int level)
    {
        textM.text = levelUpText + level;
        textM.color = textColor;
        StartCoroutine(fade());
    }

    public void notifyDeath()
    {
        textM.text = deathText;
        textM.color = textColor;
        StartCoroutine(fade());
    }

    private IEnumerator fade()
    {
        yield return new WaitForSeconds(3f);
        while(textM.color.a > 0)
        {
            textM.color = new Color(textM.color.r, textM.color.g, textM.color.b, textM.color.a - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
