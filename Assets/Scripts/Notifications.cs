using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifications : MonoBehaviour
{
    private TextMeshProUGUI textM;
    private Color textColor;


    private string levelUpText = "You have reached level ";
    private string deathText = "You have died. Restarting soon...";

    // Start is called before the first frame update
    void Start()
    {
        textM = GetComponent<TextMeshProUGUI>();
        textColor = textM.color;
    }

    // Shows a notification to the player that they have reached level X
    public void notifyLevelUp(int level)
    {
        textM.text = levelUpText + level;
        // reset the text color -> makes the text visible again
        textM.color = textColor;
        // stop previous coroutines so previous notifications do not fade the new one too early
        StopAllCoroutines();
        StartCoroutine(fade());
    }

    // Shows a notification to the player that they have died
    public void notifyDeath()
    {
        textM.text = deathText;
        // reset the text color -> makes the text visible again
        textM.color = textColor;
        // stop previous coroutines so previous notifications do not fade the new one too early
        StopAllCoroutines();
        StartCoroutine(fade());
    }

    // Fades the text into invisibility
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
