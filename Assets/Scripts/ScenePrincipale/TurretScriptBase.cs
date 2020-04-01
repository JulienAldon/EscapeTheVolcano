using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScriptBase : MonoBehaviour
{
    public Transform target;
    public Transform headpoint;
    public GameObject cannon;

    void Start() 
    {
        this.target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (!cannon.GetComponent<turretScript>().getDeactivation() && !cannon.GetComponent<turretScript>().getStoppedState()) {
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation;
            if (angle < -90 || angle > 90) {
                GetComponent<SpriteRenderer>().flipX = true;
            } else {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}

