using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{
    public GameObject[] objects;
    // Start is called before the first frame update
    private void Start()
    {
        int rand = 0;

        rand = Random.Range(0, objects.Length);
        if (objects.Length == 0)
            return;
        GameObject instance = (GameObject)Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }
}
