using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particles;
   void OnTriggerEnter2D(Collider2D other)
   {
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
   }
}
