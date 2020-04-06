﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 localPos;
	public float FireRate = 0.3f;
    float nextFire = 0;
    public Transform PointA;
    public Transform PointB;
    public Transform PointC;
    public Transform PointD;
    private bool stopped = false;
    public float step = 0.1f;

    public ExitScript ExitState;

    void Start()
    {
        localPos = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
        transform.position = localPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFire && ExitState.GetLavaFlow() && !stopped) {
            nextFire = Time.time + FireRate;
            localPos.y += step;
        }
        transform.position = localPos;

        if (ExitScript.GetState() >= 2)
        {
            FireRate = 0;
            step = 0.5f;
        } 
        
        if (transform.position.y >= PointA.position.y)
        {
            // Phase 1
            FireRate = 0.15f;
            step = 0.1f;
        }
        if (transform.position.y >= PointB.position.y)
        {
            // Phase 2
            FireRate = 0.05f;
            step = 0.2f;
        }
        if (transform.position.y >= PointC.position.y)
        {
            // Phase 3
            FireRate = 0.01f;
            step = 0.3f;
        }
        if (transform.position.y >= PointD.position.y)
        {
            stopped = true;
            // Phase 4
            // TODO: GameOver
        }
    }

    public void AddSpeed(float speed)
    {
        FireRate = speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("Player").GetComponent<CharacterStats>().LavaDie();
    }
}
