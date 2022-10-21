using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ControlNotification : MonoBehaviour
{
    private TextMeshProUGUI textM;
    private Color textColor;

    // Start is called before the first frame update
    void Start()
    {
        textM = GetComponent<TextMeshProUGUI>();
        textColor = textM.color;
        StartCoroutine(fade());

    }

    // Fades the text into invisibility
    private IEnumerator fade()
    {
        yield return new WaitForSeconds(60f);
        while (textM.color.a > 0)
        {
            textM.color = new Color(textM.color.r, textM.color.g, textM.color.b, textM.color.a - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
