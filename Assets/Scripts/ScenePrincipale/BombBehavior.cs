using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    private float startTime;
    public float detonateTime;
    public GameObject explosionEffect;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager> ();        
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
        audioManager.Play ("Explosion", UnityEngine.Random.Range (1f, 3f));						
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
