using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public Animator anim;

     void OnCollisionEnter2D(Collision2D collision)
    {
       anim.SetTrigger("isHit");
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        anim.SetTrigger("isHit");
    }
}
