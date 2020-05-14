using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : MonoBehaviour
{
    // Start is called before the first frame update
   void OnTriggerEnter2D(Collider2D other)
   {
        Destroy(gameObject);
   }
   void OnCollisionEnter2D(Collision2D collision)
   {
        Destroy(gameObject);
   }
}
