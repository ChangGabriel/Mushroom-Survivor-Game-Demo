using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private Text damageText;
    [SerializeField] private float lifeTimer;
    [SerializeField] private float upSpeed;

    private void Start()
    {
        StartCoroutine(fade());
        Destroy(gameObject, lifeTimer);
    }

    private void Update()
    {
        transform.position += new Vector3(0, upSpeed * Time.deltaTime, 0);
    }

    public void setUIDamage(float damage)
    {
        damageText.text = damage.ToString(); 
    }

    // Fades the text into invisibility
    private IEnumerator fade()
    {
        yield return new WaitForSeconds(0.4f);
        while (damageText.color.a > 0)
        {
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, damageText.color.a - 0.07f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
