using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpFx : MonoBehaviour
{
    [SerializeField] private float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
