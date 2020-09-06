using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public GameObject DestroyEffect;
    public float acceleration = 0.5f;
    public float maxSpeed = 2.0f;
    public Vector2 direction;
    public AudioSource explosion;
    private bool once;

    private float angle;
    private float curSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        once = false;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.Rotate(new Vector3(0, 0, angle + 90));        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(direction.x, direction.y) * curSpeed;
//        transform.Translate(Vector3.forward * curSpeed);
        curSpeed += acceleration;
        if (curSpeed > maxSpeed)
            curSpeed = maxSpeed;        
    //        GetComponent<Rigidbody2D>().AddForce(transform.forward * 5);
        }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!once) {
            once = true;
            explosion.Play (0);
            GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(DestroyEffect, transform.position, Quaternion.identity);
            Destroy (gameObject, 0.5f);
        }
    }
}
