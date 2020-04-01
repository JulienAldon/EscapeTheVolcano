using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    private float startTime;
    public float detonateTime;
    public GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= detonateTime) {
            explode();
        }
    }

    public void explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

/*    void OnTriggerEnter2D(Collider2D collision)
    {
        int LayerIndex = LayerMask.NameToLayer("Bombs");
        int layerMask = (1 << LayerIndex);

        if (collision.IsTouchingLayers(layerMask)) {
            explode();
        }
    }*/

    void OnCollisionEnter2D(Collision2D collision)
    {
        int LayerIndex = LayerMask.NameToLayer("Bombs");
        int layerMask = (1 << LayerIndex);

        if (collision.collider.IsTouchingLayers(layerMask)) {
            explode();
        }
    }
}
