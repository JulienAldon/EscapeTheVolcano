using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 19 || collision.gameObject.layer == 12) {
            Destroy(gameObject);
        }
    }

    // void OnTriggerEnter2D(Collider2D collision) 
    // {
    // }
}
