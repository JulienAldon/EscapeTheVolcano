using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolScript : MonoBehaviour
{
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public float speed;
    public BoxCollider2D col;
    void Update() {
        // move();
         //RaycastHit2D groundInfo = Physics2D.Raycast(GroundDetection.position, Vector2.down, distance);
        Collider2D[] r = new Collider2D[10];
        ContactFilter2D f = new ContactFilter2D();
        f.layerMask = LayerMask.GetMask("Ground");
        int a = col.OverlapCollider(f, r);
        if (col.IsTouchingLayers(LayerMask.GetMask("Ground")) == false || a > 2)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
        }
        if (IsFacingRight()) {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
    
}
