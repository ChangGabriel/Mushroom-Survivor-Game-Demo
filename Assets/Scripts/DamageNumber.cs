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
}
