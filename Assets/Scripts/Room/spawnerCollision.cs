using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerCollision : MonoBehaviour
{
    public bool outsideBounds;

    private void Start()
    {
        outsideBounds = false;
    }

    // Handles if the spawner is outside of level bounds
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("LevelBorder"))
        {
            if (!outsideBounds)
            {
                outsideBounds = true;
            }
            else
            {
                outsideBounds = false;
            }
            transform.parent.GetComponentInParent<waveManager>().toggleSpawner(outsideBounds, this.gameObject);
        }
    }
}
