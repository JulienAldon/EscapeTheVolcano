using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject DestroyEffect;
    public int maxBounce;
    private int currentBounce;

    void Start()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentBounce >= maxBounce || collision.gameObject.layer != 8)
        {
            Destroy (gameObject);
            Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        }
        currentBounce += 1;
    }
}
