using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameObject notifier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject getNotifier()
    {
        return notifier;
    }
}
