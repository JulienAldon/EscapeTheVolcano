using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        if (!cam)
           cam = GameObject.Find("Main Camera");
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * smoothing);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
