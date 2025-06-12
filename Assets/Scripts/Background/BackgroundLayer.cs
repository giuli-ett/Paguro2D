using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundLayer : MonoBehaviour
{
    private float startPos;
    private float lenght;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxFactor;
    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxFactor;
        float movement = cam.transform.position.x * (1 - parallaxFactor);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        if(movement > startPos + lenght)
        {
            startPos += lenght;
        }
        else if(movement < startPos - lenght)
        {
            startPos -= lenght;
        }
    }
}
