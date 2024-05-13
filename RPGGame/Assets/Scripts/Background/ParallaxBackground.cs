using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float lenght;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position=new Vector3(xPosition + distanceToMove, transform.position.y);


        if (distanceMoved > xPosition + lenght)
            xPosition = xPosition + lenght;
        else if(distanceMoved < xPosition - lenght)
            xPosition = xPosition - lenght;
    }
}
