using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ParrallaxLights : MonoBehaviour
{
    private float lengthX, lengthY, xpos, ypos;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        xpos = transform.position.x;
        ypos = transform.position.y;
        lengthX = 40;
        lengthY = 30;
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = (cam.transform.position.x * (1 - parallaxEffect));
        float tempY = (cam.transform.position.y * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        float ydist = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector3(xpos + dist, ypos + ydist, transform.position.z);
        if (tempX > xpos + lengthX)
        {
            xpos += lengthX * 2;
        }
        else if (tempX < xpos - lengthX)
        {
            xpos -= lengthX * 2;
        }

        if (tempY > ypos + lengthY)
        {
            ypos += lengthY * 2;
        }
        else if (tempY < ypos - lengthY)
        {
            ypos -= lengthY * 2;
        }
    }
}