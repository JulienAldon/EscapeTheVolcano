using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSpawn : MonoBehaviour
{
    public GameObject lava;
    public float lavaSpawnRate;
    private float nextSpawn = 0;
    private int whereIsLava;
    // Start is called before the first frame update
    void Start()
    {
        Level.path.Reverse();
        whereIsLava = 0;
        nextSpawn = Time.time + lavaSpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn && Level.canWin == true) {
            nextSpawn = Time.time + lavaSpawnRate;
            Instantiate(lava, new Vector3(Level.path[whereIsLava].transform.position.x, Level.path[whereIsLava].transform.position.y, -1f), Quaternion.identity);
            if (whereIsLava != Level.path.Count - 1)
                whereIsLava += 1;
        }
        if (whereIsLava >= Level.path.Count)
        {
            // Display a message, press any key to continue -> GAME OVER SCREENs
        }
    }
}
