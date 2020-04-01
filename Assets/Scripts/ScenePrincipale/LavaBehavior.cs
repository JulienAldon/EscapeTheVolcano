using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 localScale;
    Vector3 localPos;
	public float FireRate = 0f;
    float nextFire = 0;

    void Start()
    {
        localPos = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
        localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
        transform.localScale = localScale;
        transform.position = localPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (localScale.y < 10)
        {
            nextFire = Time.time + FireRate;
            localScale.y += 0.1f;
            localPos.y += 0.05f;
        }
        transform.localScale = localScale;
        transform.position = localPos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("Player").GetComponent<CharacterStats>().LavaDie();
        print("PLAYER DIED");
    }
}
