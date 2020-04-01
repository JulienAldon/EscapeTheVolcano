using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public float velX = 5f;
    public float velY = 0f;
    Rigidbody2D rb;
    public GameObject DestroyEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velX, velY);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy (gameObject);
        Instantiate(DestroyEffect, transform.position, Quaternion.identity);
    }
}
