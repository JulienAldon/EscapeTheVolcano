﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float laserTime = 4f;
    public Vector2 direction;
    public GameObject Bullet;

    private LineRenderer _line;
    private float angle;
    private float timeSpawn;
    private int once = 0;
    private Vector2 laserPosition;
    private Vector3 lastLaserPosition;
    private bool stopped = false;

    void Start ()
    {
        timeSpawn = Time.time + 1;
        Destroy (gameObject, laserTime);
    }

    public bool getStoppedState() {
        return stopped;
    }
    
    void Update()
    {
        Transform target = GameObject.FindWithTag("Player").transform;
        direction = target.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _line = gameObject.GetComponent<LineRenderer>();
        _line.SetPosition(0, transform.position);
        _line.SetPosition(0, transform.parent.GetChild(0).position);
        
        if (Time.time < timeSpawn - 0.2f) {
            stopped = false;
            int layerMask = (LayerMask.GetMask("Ground", "Player"));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);
            if (hit) {
                laserPosition = hit.point;
                _line.SetPosition(1, laserPosition);
                lastLaserPosition = laserPosition;
            }
        } else {
            stopped = true;
            _line.SetPosition(1, lastLaserPosition);
            _line.enabled = false;
        }
        int layerMaskP = (LayerMask.GetMask("Player"));
        if (Time.time > timeSpawn && once == 0) {
            once = 1;
            Vector2 newdirection = lastLaserPosition - transform.position;
            GameObject a = Instantiate(Bullet, transform.parent.GetChild(0).position, Quaternion.identity);
            a.GetComponent<TurretBullet>().direction = newdirection.normalized;
        }
    }
}
